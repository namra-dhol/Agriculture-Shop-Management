using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text.Json;
using AgriShop_Consume.Models;
using AgriShop_Consume.Helper;

public class RegisterController : Controller
{
    private readonly HttpClient _client;

    public RegisterController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient("ApiClient");
    }

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(RegisterViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var jsonDebug = JsonSerializer.Serialize(model);
            Console.WriteLine("Sending Register JSON: " + jsonDebug);

            var response = await _client.PostAsJsonAsync("Auth/register", model);
            var rawResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API Raw Response: " + rawResponse);
            Console.WriteLine("Status Code: " + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                // Optional: Add TempData for success message
                TempData["Success"] = "Registration successful! Please log in.";
                return RedirectToAction("Index", "Login");
            }

            TempData["Error"] = "Registration failed. Email might already be in use.";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Exception during registration: " + ex.Message);
            TempData["Error"] = "An unexpected error occurred.";
        }

        return View(model);
    }
}
