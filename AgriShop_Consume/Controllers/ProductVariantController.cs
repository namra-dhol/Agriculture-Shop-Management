using System.Text;
using AgriShop_Consume.Models;
using AgriShop_Consume.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AgriShop_Consume.Controllers
{
    [Authorize]
    public class ProductVariantController : Controller
    {
        private readonly HttpClient _client;

        public ProductVariantController(IHttpClientFactory httpClientFactory)
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

        // List all product variants with optional filtering
        public async Task<IActionResult> ProductVariantList(int? productId, string? size)
        {
            SetBearerToken();
            
            string endpoint = "ProductVariant";
            var queryParams = new List<string>();
            
            if (productId.HasValue)
            {
                queryParams.Add($"productId={productId.Value}");
            }
            
            if (!string.IsNullOrEmpty(size))
            {
                queryParams.Add($"size={Uri.EscapeDataString(size)}");
            }
            
            if (queryParams.Any())
            {
                endpoint = $"ProductVariant/Filter?{string.Join("&", queryParams)}";
            }
            
            var response = await _client.GetAsync(endpoint);
            
            if (!response.IsSuccessStatusCode)
            {
                // If filtering fails, fall back to getting all product variants
                if (queryParams.Any())
                {
                    response = await _client.GetAsync("ProductVariant");
                }
                else
                {
                    return View(new List<ProductVariantModel>());
                }
            }
            
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<ProductVariantModel>>(json) ?? new List<ProductVariantModel>();
            
            // Pass the search parameters to the view for maintaining the search inputs
            ViewBag.ProductId = productId;
            ViewBag.Size = size;
            
            // Get products for dropdown
            ViewBag.ProductList = await GetDropdown("Product");
            
            return View(list);
        }

        // Delete product variant by ID
        public async Task<IActionResult> Delete(int id)
        {
            SetBearerToken();
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
                SetBearerToken();
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
            SetBearerToken();
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
            SetBearerToken();
            model.ProductList = await GetDropdown("Product");
        }

         private async Task<List<SelectListItem>> GetDropdown(string endpoint)
        {
            List<SelectListItem> items = new();

            try
            {
                SetBearerToken();
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