using Microsoft.AspNetCore.Mvc;
using AgriShop_Consume.Models;
using AgriShop_Consume.Helper;
using System.Text.Json;

namespace AgriShop_Consume.Controllers
{
    public class QueryController : Controller
    {
        private readonly HttpClient _client;

        public QueryController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5275/api/");
        }

        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> SendQuery([FromBody] QueryRequestModel model)
        {
            try
            {
                Console.WriteLine($"MVC QueryController: Sending query - ProductTypeId={model.ProductTypeId}, ProductId={model.ProductId}, VariantId={model.VariantId}");
                
                var response = await _client.PostAsJsonAsync("Query/send", model);

                Console.WriteLine($"MVC QueryController: Response status = {response.StatusCode}");

                if (response.IsSuccessStatusCode)
                {
                    var result = await response.Content.ReadFromJsonAsync<QueryResponseModel>();
                    Console.WriteLine($"MVC QueryController: Success - {result?.Message}");
                    return Json(new { success = true, message = result?.Message });
                }
                else
                {
                    var errorContent = await response.Content.ReadAsStringAsync();
                    Console.WriteLine($"MVC QueryController: Error - {errorContent}");
                    return Json(new { success = false, message = $"Error: {errorContent}" });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"MVC QueryController: Exception - {ex.Message}");
                return Json(new { success = false, message = $"Exception: {ex.Message}" });
            }
        }
    }
}
