using AgriShop_Consume.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Mvc;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Text;

public class ProductController : Controller
{
    private readonly HttpClient _httpClient;

    public ProductController()
    {
        _httpClient = new HttpClient
        {
            BaseAddress = new Uri("http://localhost:5275/api/")
        };
        _httpClient.DefaultRequestHeaders.Accept.Clear();
        _httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
    }

    public async Task<IActionResult> ProductList()
    {
        List<ProductModel> products = new();
        HttpResponseMessage response = await _httpClient.GetAsync("Product");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            products = JsonSerializer.Deserialize<List<ProductModel>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        return View(products);
    }

    public async Task<IActionResult> ProductAdd()
    {
        var product = new ProductModel();
        await PopulateDropdowns(product);
        return View("ProductForm", product);
    }

    [HttpPost]
    public async Task<IActionResult> ProductAdd(ProductModel product)
    {
        if (!ModelState.IsValid)
        {
            await PopulateDropdowns(product);
            return View("ProductForm", product);
        }

        try
        {

            if (product.ProductId == 0)
                product.CreatedAt = DateTime.Now;
            else
                product.ModifiedAt = DateTime.Now;

            var jsonData = JsonSerializer.Serialize(product);
            var content = new StringContent(jsonData, Encoding.UTF8, "application/json");

            HttpResponseMessage response;

            if (product.ProductId == 0)
                response = await _httpClient.PostAsync("Product", content);
            else
                response = await _httpClient.PutAsync($"Product/{product.ProductId}", content);

            if (response.IsSuccessStatusCode)
                return RedirectToAction("ProductList");

            ModelState.AddModelError("", "Failed to save product.");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", $"Error: {ex.Message}");
        }

        await PopulateDropdowns(product);
        return View("ProductForm", product);
    }

    public async Task<IActionResult> Edit(int id)
    {
        ProductModel product = null;
        var response = await _httpClient.GetAsync($"Product/{id}");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            product = JsonSerializer.Deserialize<ProductModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        if (product == null)
            return NotFound();

        await PopulateDropdowns(product);
        return View("ProductForm", product);
    }

    public async Task<IActionResult> Delete(int id)
    {
        var response = await _httpClient.DeleteAsync($"Product/{id}");
        TempData["Message"] = response.IsSuccessStatusCode ? "Deleted" : "Failed to delete";
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

        var response = await _httpClient.GetAsync(endpoint);
        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            var rawItems = JsonSerializer.Deserialize<List<Dictionary<string, object>>>(data);

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
        List<ProductModel> products = new();
        HttpResponseMessage response = await _httpClient.GetAsync($"Product/bytype-simple/{productTypeId}");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            products = JsonSerializer.Deserialize<List<ProductModel>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Get product type name for display
        var productTypeResponse = await _httpClient.GetAsync($"ProductType/{productTypeId}");
        string productTypeName = "Products";
        if (productTypeResponse.IsSuccessStatusCode)
        {
            var typeData = await productTypeResponse.Content.ReadAsStringAsync();
            var productType = JsonSerializer.Deserialize<ProductTypeModel>(typeData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            productTypeName = productType?.TypeName ?? "Products";
        }

        ViewBag.ProductTypeName = productTypeName;
        ViewBag.ProductTypeId = productTypeId;

        return View(products);
    }

    public async Task<IActionResult> ProductDetail(int productId)
    {
        ProductWithVariantsModel product = null;
        var response = await _httpClient.GetAsync($"Product/with-variants/{productId}");

        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            product = JsonSerializer.Deserialize<ProductWithVariantsModel>(json, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        if (product == null)
            return NotFound();

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
        List<ProductModel> products = new();
        HttpResponseMessage response = await _httpClient.GetAsync($"Product/bytype-simple/{productTypeId}");

        if (response.IsSuccessStatusCode)
        {
            var data = await response.Content.ReadAsStringAsync();
            products = JsonSerializer.Deserialize<List<ProductModel>>(data, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }

        // Get product type name for display
        var productTypeResponse = await _httpClient.GetAsync($"ProductType/{productTypeId}");
        string productTypeName = "Products";
        if (productTypeResponse.IsSuccessStatusCode)
        {
            var typeData = await productTypeResponse.Content.ReadAsStringAsync();
            var productType = JsonSerializer.Deserialize<ProductTypeModel>(typeData, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
            productTypeName = productType?.TypeName ?? "Products";
        }

        ViewBag.ProductTypeName = productTypeName;
        ViewBag.ProductTypeId = productTypeId;

        return View("ProductsByType", products);
    }
}

