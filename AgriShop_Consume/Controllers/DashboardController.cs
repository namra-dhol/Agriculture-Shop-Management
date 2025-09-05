using System.Net.Http;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using AgriShop_Consume.Models;
using AgriShop_Consume.Helper;
using Newtonsoft.Json;
using System.Net.Http.Headers;

[Authorize]
public class DashboardController : Controller
{
    private readonly HttpClient _client;

    public DashboardController(IHttpClientFactory httpClientFactory)
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

    public async Task<IActionResult> Index()
    {
        SetBearerToken();
        DashboardModel model = new DashboardModel();
        var response = await _client.GetAsync("dashboard/summary");
        if (response.IsSuccessStatusCode)
        {
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<DashboardModel>(json);
            if (data != null)
                model = data;
        }
        return View(model);
    }
}


