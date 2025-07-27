using System.Text;
using AgriShop_Consume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgriShop_Consume.Controllers
{
    public class SupplierController : Controller
    {
        private readonly HttpClient _client;

        public SupplierController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5275/api/");
        }


        // List all suppliers
        public async Task<IActionResult> SupplierList()
        {
            var response = await _client.GetAsync("Supplier");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<SupplierModel>>(json);
            return View(list);
        }

        // Delete supplier by ID
        public async Task<IActionResult> Delete(int id)
        {
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