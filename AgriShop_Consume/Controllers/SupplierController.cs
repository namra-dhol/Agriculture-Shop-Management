using System.Text;
using AgriShop_Consume.Models;
using AgriShop_Consume.Helper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Newtonsoft.Json;
using System.Net.Http.Headers;

namespace AgriShop_Consume.Controllers
{
    [Authorize]
    public class SupplierController : Controller
    {
        private readonly HttpClient _client;

        public SupplierController(IHttpClientFactory httpClientFactory)
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


        // List all suppliers
        public async Task<IActionResult> SupplierList()
        {
            SetBearerToken();
            var response = await _client.GetAsync("Supplier");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<SupplierModel>>(json);
            return View(list);
        }

        // Delete supplier by ID
        public async Task<IActionResult> Delete(int id)
        {
            SetBearerToken();
            await _client.DeleteAsync($"Supplier/{id}");
            return RedirectToAction("SupplierList");
        }

        // GET: Add/Edit Supplier
        public async Task<IActionResult> AddEdit(int? id)
        {
            SupplierModel supplier;
            if (id == null)
            {
                supplier = new SupplierModel();
            }
            else
            {
                SetBearerToken();
                var response = await _client.GetAsync($"Supplier/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }
                var json = await response.Content.ReadAsStringAsync();
                supplier = JsonConvert.DeserializeObject<SupplierModel>(json);
            }
            return View(supplier);
        }

        [HttpPost]
        public async Task<IActionResult> AddEdit(SupplierModel supplier)
        {
            if (!ModelState.IsValid)
            {
                return View(supplier);
            }
            SetBearerToken();
            var content = new StringContent(JsonConvert.SerializeObject(supplier), Encoding.UTF8, "application/json");
            if (supplier.SupplierId > 0)
            {
                // Update existing supplier
                await _client.PutAsync($"Supplier/{supplier.SupplierId}", content);
            }
            else
            {
                // Create new supplier
                await _client.PostAsync("Supplier", content);
            }
            return RedirectToAction("SupplierList");
        }
    }
} 