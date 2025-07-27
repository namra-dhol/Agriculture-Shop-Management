using System.Text;
using AgriShop_Consume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgriShop_Consume.Controllers
{
    public class ProductTypeController : Controller
    {
        private readonly HttpClient _client;

        public ProductTypeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5275/api/");
        }

        // List all product types
        public async Task<IActionResult> ProductTypeList()
        {
            var response = await _client.GetAsync("ProductType");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ProductTypeModel>>(json);
            return View(list);
        }

        // Delete product type by ID
        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsync($"ProductType/{id}");
            return RedirectToAction("ProductTypeList");
        }

        // GET: Add/Edit Product Type
        public async Task<IActionResult> AddEdit(int? id)
        {
            ProductTypeModel productType;
            if (id == null)
            {
                productType = new ProductTypeModel();
            }
            else
            {
                var response = await _client.GetAsync($"ProductType/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }
                var json = await response.Content.ReadAsStringAsync();
                productType = JsonConvert.DeserializeObject<ProductTypeModel>(json);
            }
            return View(productType);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(ProductTypeModel productType)
        {
            if (!ModelState.IsValid)
            {
                return View(productType);
            }
            var content = new StringContent(JsonConvert.SerializeObject(productType), Encoding.UTF8, "application/json");
            if (productType.ProductTypeId > 0)
            {
                // Update existing product type
                await _client.PutAsync($"ProductType/{productType.ProductTypeId}", content);
            }
            else
            {
                // Create new product type
                await _client.PostAsync("ProductType", content);
            }
            return RedirectToAction("ProductTypeList");
        }
    }
} 