using System.Data;
using System.Text;
using AgriShop_Consume.Models;
using AgriShop_Consume.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AgriShop_Consume.Controllers
{
  

    [Authorize]
    public class ProductTypeController : Controller
    {
        private readonly HttpClient _client;

        public ProductTypeController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5275/api/");
        }

        private void SetBearerToken()
        {
            if (!string.IsNullOrWhiteSpace(TokenManager.Token))
            {
                _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TokenManager.Token);
            }
        }

        // List all product types with optional filtering
        public async Task<IActionResult> ProductTypeList(string? typeName)
        {
            SetBearerToken();
            
            string endpoint = "ProductType";
            if (!string.IsNullOrEmpty(typeName))
            {
                endpoint = $"ProductType/Filter?typeName={Uri.EscapeDataString(typeName)}";
            }
            
            var response = await _client.GetAsync(endpoint);
            
            if (!response.IsSuccessStatusCode)
            {
                // If filtering fails, fall back to getting all product types
                if (!string.IsNullOrEmpty(typeName))
                {
                    response = await _client.GetAsync("ProductType");
                }
                else
                {
                    return View(new List<ProductTypeModel>());
                }
            }
            
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ProductTypeModel>>(json) ?? new List<ProductTypeModel>();
            
            // Pass the search term to the view for maintaining the search input
            ViewBag.SearchTerm = typeName;
            
            return View(list);
        }

        // Delete product type by ID
        public async Task<IActionResult> Delete(int id)
        {
            SetBearerToken();
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
                SetBearerToken();
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
            SetBearerToken();
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