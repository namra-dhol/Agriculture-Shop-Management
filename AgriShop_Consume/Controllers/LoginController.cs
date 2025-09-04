using Microsoft.AspNetCore.Mvc;
using AgriShop_Consume.Models;
using AgriShop_Consume.Helper;
using System.Text.Json;

public class LoginController : Controller
{
    private readonly HttpClient _client;

    public LoginController(IHttpClientFactory httpClientFactory)
    {
        _client = httpClientFactory.CreateClient();
        _client.BaseAddress = new Uri("http://localhost:5275/api/");
    }

    public IActionResult Index() => View();

    [HttpPost]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        try
        {
            var jsonDebug = JsonSerializer.Serialize(model);
            Console.WriteLine("Sending Login JSON: " + jsonDebug);

            var response = await _client.PostAsJsonAsync("Auth/login", model);

            var rawResponse = await response.Content.ReadAsStringAsync();
            Console.WriteLine("API Raw Response: " + rawResponse);
            Console.WriteLine("Status Code: " + response.StatusCode);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<LoginResponse>() ?? new LoginResponse();
                TokenManager.Token = result.Token;
                TokenManager.Role = result.Role;
                TokenManager.Username = result.Username; // ✅ changed from Email to Username

              
                    TempData["Success"] = "Successfully logged in as Admin!";
                    return Redirect("/Home/Index");
               
                // You can redirect other roles too
            }

            TempData["Error"] = "Invalid username or password.";
        }
        catch (Exception ex)
        {
            Console.WriteLine("Error: " + ex.Message);
            TempData["Error"] = "An unexpected error occurred.";
        }

        return View(model);
    }
}
