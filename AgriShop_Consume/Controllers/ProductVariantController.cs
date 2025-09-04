using System.Text;
using AgriShop_Consume.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;

namespace AgriShop_Consume.Controllers
{
    public class ProductVariantController : Controller
    {
        private readonly HttpClient _client;

        public ProductVariantController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5275/api/");
        }

        // List all product variants
        public async Task<IActionResult> ProductVariantList()
        {
            var response = await _client.GetAsync("ProductVariant");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ProductVariantModel>>(json);
            return View(list);
        }

        // Delete product variant by ID
        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsync($"ProductVariant/{id}");
            return RedirectToAction("ProductVariantList");
        }

        // GET: Add/Edit Product Variant
        public async Task<IActionResult> AddEdit(int? id)
        {
            ProductVariantModel productVariant;
            if (id == null)
            {
                productVariant = new ProductVariantModel();
            }
            else
            {
                var response = await _client.GetAsync($"ProductVariant/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }
                var json = await response.Content.ReadAsStringAsync();
                productVariant = JsonConvert.DeserializeObject<ProductVariantModel>(json);
            }

            // Populate dropdowns
            await PopulateDropdowns(productVariant);
            return View(productVariant);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductVariantModel productVariant)
        {
            if (!ModelState.IsValid)
            {
                // Repopulate dropdowns if validation fails
                await PopulateDropdowns(productVariant);
                return View(productVariant);
            }
            var content = new StringContent(JsonConvert.SerializeObject(productVariant), Encoding.UTF8, "application/json");
            if (productVariant.VariantId > 0)
            {
                // Update existing product variant
                await _client.PutAsync($"ProductVariant/{productVariant.VariantId}", content);
            }
            else
            {
                // Create new product variant
                await _client.PostAsync("ProductVariant", content);
            }
            return RedirectToAction("ProductVariantList");
        }

        // Helper for dropdowns
        private async Task PopulateDropdowns(ProductVariantModel model)
        {
            model.ProductList = await GetDropdown("Product");
        }

         private async Task<List<SelectListItem>> GetDropdown(string endpoint)
        {
            List<SelectListItem> items = new();

            try
            {
                // Call the main Product API directly instead of the dropdown endpoint
                var response = await _client.GetAsync("Product");
                if (response.IsSuccessStatusCode)
                {
                    var data = await response.Content.ReadAsStringAsync();
                    var products = JsonConvert.DeserializeObject<List<ProductModel>>(data);

                    if (products != null)
                    {
                        foreach (var product in products)
                        {
                            items.Add(new SelectListItem
                            {
                                Value = product.ProductId.ToString(),
                                Text = product.ProductName ?? "Unknown Product"
                            });
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                // // If API fails, add some dummy data for testing
                // items.Add(new SelectListItem { Value = "1", Text = "Test Product 1" });
                // items.Add(new SelectListItem { Value = "2", Text = "Test Product 2" });
                // items.Add(new SelectListItem { Value = "3", Text = "Test Product 3" });
                Console.WriteLine(ex.Message);
            }
            return items;
        }
 

    }
} 