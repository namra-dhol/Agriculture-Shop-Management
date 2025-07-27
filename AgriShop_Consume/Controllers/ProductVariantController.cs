using System.Text;
using AgriShop_Consume.Models;
using Microsoft.AspNetCore.Mvc;
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
            return View(productVariant);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductVariantModel productVariant)
        {
            if (!ModelState.IsValid)
            {
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
    }
} 