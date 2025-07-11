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

}

