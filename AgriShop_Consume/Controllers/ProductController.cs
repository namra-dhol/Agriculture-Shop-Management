using AgriShop_Consume.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;
using Newtonsoft.Json;

public class ProductController : Controller
{
    private readonly HttpClient _client;
    private readonly IWebHostEnvironment _webHostEnvironment;

    public ProductController(IHttpClientFactory httpClientFactory, IWebHostEnvironment webHostEnvironment)
    {
        _client = httpClientFactory.CreateClient();
        _client.BaseAddress = new Uri("http://localhost:5275/api/");
        _webHostEnvironment = webHostEnvironment;
    }

    public async Task<IActionResult> ProductList()
    {
        var response = await _client.GetAsync("Product");
        var json = await response.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<List<ProductModel>>(json);
        return View(products);
    }

    public async Task<IActionResult> ProductAdd()
    {
        var product = new ProductModel();
        await PopulateDropdowns(product);
        return View("ProductForm", product);
    }

    [HttpPost]
    public async Task<IActionResult> ProductAdd(ProductModel product, IFormFile? productImage, string? selectedImagePath)
    {
        // Require image for new product (either file upload or sample selection)
        if (product.ProductId == 0 && (productImage == null || productImage.Length == 0) && string.IsNullOrEmpty(selectedImagePath))
        {
            ModelState.AddModelError("productImage", "Please select an image for the product (upload file or choose from sample images).");
        }

        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(product);
            return View("ProductForm", product);
        }

        try
        {
            // Handle image upload or selection
            if (productImage != null && productImage.Length > 0)
            {
                // Save uploaded file
                product.ProductImg = await SaveUploadedImage(productImage);
                System.Diagnostics.Debug.WriteLine($"Uploaded image: {product.ProductImg}");
            }
            else if (!string.IsNullOrEmpty(selectedImagePath))
            {
                // Use selected sample image
                product.ProductImg = selectedImagePath;
                System.Diagnostics.Debug.WriteLine($"Selected image path: {product.ProductImg}");
            }
            // else: If editing and no new image selected, keep existing image (do nothing)

            if (product.ProductId == 0)
                product.CreatedAt = DateTime.Now;
            else
                product.ModifiedAt = DateTime.Now;

            var content = new StringContent(JsonConvert.SerializeObject(product), Encoding.UTF8, "application/json");

            HttpResponseMessage response;
            if (product.ProductId > 0)
            {
                // Update existing product
                response = await _client.PutAsync($"Product/{product.ProductId}", content);
            }
            else
            {
                // Create new product
                response = await _client.PostAsync("Product", content);
            }

            if (response.IsSuccessStatusCode)
            {
                TempData["Success"] = product.ProductId > 0 ? "Product updated successfully!" : "Product created successfully!";
                return RedirectToAction("ProductList");
            }
            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                ModelState.AddModelError("", $"Failed to save product. Status: {response.StatusCode}, Error: {errorContent}");
            }
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error: {ex.Message}");
        }

        await PopulateDropdowns(product);
        return View("ProductForm", product);
    }

    private async Task<string> SaveUploadedImage(IFormFile imageFile)
    {
        try
        {
            // Create uploads directory if it doesn't exist
            var uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            // Generate unique filename
            var fileName = Guid.NewGuid().ToString() + Path.GetExtension(imageFile.FileName);
            var filePath = Path.Combine(uploadsFolder, fileName);

            // Save the file
            using (var stream = new FileStream(filePath, FileMode.Create))
            {
                await imageFile.CopyToAsync(stream);
            }

            // Return the relative path for the database
            return "/uploads/" + fileName;
        }
        catch (Exception ex)
        {
            throw new Exception($"Error saving image: {ex.Message}");
        }
    }

    public async Task<IActionResult> Edit(int id)
    {
        var response = await _client.GetAsync($"Product/{id}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        var product = JsonConvert.DeserializeObject<ProductModel>(json);

        await PopulateDropdowns(product);
        return View("ProductForm", product);
    }

    public async Task<IActionResult> Delete(int id)
    {
        await _client.DeleteAsync($"Product/{id}");
        TempData["Message"] = "Product deleted successfully";
        return RedirectToAction("ProductList");
    }

    // Helper for dropdowns
    private async Task PopulateDropdowns(ProductModel model)
    {
        model.ProductTypeList = await GetDropdown("product/dropdown/producttypes");
        model.SupplierList = await GetDropdown("product/dropdown/suppliers");
        model.UserList = await GetDropdown("product/dropdown/users");
    }

    private async Task<List<SelectListItem>> GetDropdown(string endpoint)
    {
        List<SelectListItem> items = new();

        var response = await _client.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var rawItems = JsonConvert.DeserializeObject<List<Dictionary<string, object>>>(data);

            foreach (var item in rawItems)
            {
                items.Add(new SelectListItem
                {
                    Value = item.First().Value.ToString(),
                    Text = item.Last().Value.ToString()
                });
            }
        }

        return items;
    }

    // New methods for product browsing functionality
    public async Task<IActionResult> ProductsByType(int productTypeId)
    {
        var response = await _client.GetAsync($"Product/bytype-simple/{productTypeId}");
        var json = await response.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<List<ProductModel>>(json);

        // Get product type name for display
        var productTypeResponse = await _client.GetAsync($"ProductType/{productTypeId}");
        string productTypeName = "Products";
        if (productTypeResponse.IsSuccessStatusCode)
        {
            var typeData = await productTypeResponse.Content.ReadAsStringAsync();
            var productType = JsonConvert.DeserializeObject<ProductTypeModel>(typeData);
            productTypeName = productType?.TypeName ?? "Products";
        }

        ViewBag.ProductTypeName = productTypeName;
        ViewBag.ProductTypeId = productTypeId;

        return View(products);
    }

    public async Task<IActionResult> ProductDetail(int productId)
    {
        var response = await _client.GetAsync($"Product/with-variants/{productId}");
        if (!response.IsSuccessStatusCode)
        {
            return NotFound();
        }

        var json = await response.Content.ReadAsStringAsync();
        var product = JsonConvert.DeserializeObject<ProductWithVariantsModel>(json);

        return View(product);
    }

    // Default product type browsing (e.g., Fertilizers & Chemicals)
    public async Task<IActionResult> DefaultProducts(int productTypeId = 1)
    {
        // You can change the default productTypeId parameter to match your desired category
        // For example: DefaultProducts(2) for Seeds, DefaultProducts(3) for Tools, etc.
        return await ProductsByType(productTypeId);
    }

    // Specific product type methods for easy navigation
    public async Task<IActionResult> FertilizersAndChemicals()
    {
        // Change this ID to match your "Fertilizers & Chemicals" product type
        return await GetProductsByTypeWithView(1);
    }

    public async Task<IActionResult> Seeds()
    {
        // Change this ID to match your "Seeds" product type
        return await GetProductsByTypeWithView(2);
    }

    public async Task<IActionResult> FarmTools()
    {
        // Change this ID to match your "Farm Tools" product type
        return await GetProductsByTypeWithView(3);
    }

    public async Task<IActionResult> Irrigation()
    {
        // Change this ID to match your "Irrigation" product type
        return await GetProductsByTypeWithView(4);
    }

    public async Task<IActionResult> Pesticides()
    {
        // Change this ID to match your "Pesticides" product type
        return await GetProductsByTypeWithView(5);
    }

    public async Task<IActionResult> GardenCare()
    {
        // Change this ID to match your "Garden Care" product type
        return await GetProductsByTypeWithView(6);
    }

    // Helper method to get products by type and return the correct view
    private async Task<IActionResult> GetProductsByTypeWithView(int productTypeId)
    {
        var response = await _client.GetAsync($"Product/bytype-simple/{productTypeId}");
        var json = await response.Content.ReadAsStringAsync();
        var products = JsonConvert.DeserializeObject<List<ProductModel>>(json);

        // Get product type name for display
        var productTypeResponse = await _client.GetAsync($"ProductType/{productTypeId}");
        string productTypeName = "Products";
        if (productTypeResponse.IsSuccessStatusCode)
        {
            var typeData = await productTypeResponse.Content.ReadAsStringAsync();
            var productType = JsonConvert.DeserializeObject<ProductTypeModel>(typeData);
            productTypeName = productType?.TypeName ?? "Products";
        }

        ViewBag.ProductTypeName = productTypeName;
        ViewBag.ProductTypeId = productTypeId;

        return View("ProductsByType", products);
    }
}

