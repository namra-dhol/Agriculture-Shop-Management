using System.Text;
using AgriShop_Consume.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace AgriShop_Consume.Controllers
{
    public class UserController : Controller
    {
        private readonly HttpClient _client;

        public UserController(IHttpClientFactory httpClientFactory)
        {
            _client = httpClientFactory.CreateClient();
            _client.BaseAddress = new Uri("http://localhost:5275/api/"); 
        }

        // List all users
        public async Task<IActionResult> UserList()
        {
            var response = await _client.GetAsync("User");
            var json = await response.Content.ReadAsStringAsync();
            var list = JsonConvert.DeserializeObject<List<UserModel>>(json);

            return View(list);
        }

        // Delete user by ID
        public async Task<IActionResult> Delete(int id)
        {
            await _client.DeleteAsync($"User/{id}");
            return RedirectToAction("UserList");
        }

        // GET: Add/Edit User
        public async Task<IActionResult> AddEdit(int? id)
        {
            UserModel user;

            if (id == null)
            {
                user = new UserModel();
            }
            else
            {
                var response = await _client.GetAsync($"User/{id}");
                if (!response.IsSuccessStatusCode)
                {
                    return NotFound();
                }

                var json = await response.Content.ReadAsStringAsync();
                user = JsonConvert.DeserializeObject<UserModel>(json);
            }

            return View(user);
        }

     
        [HttpPost]
        public async Task<IActionResult> AddEdit(UserModel user)
        {
            if (!ModelState.IsValid)
            {
                return View(user);
            }

            var content = new StringContent(JsonConvert.SerializeObject(user), Encoding.UTF8, "application/json");

            if (user.UserId > 0)
            {
                // Update existing user
                await _client.PutAsync($"User/{user.UserId}", content);
            }
            else
            {
                // Create new user
                await _client.PostAsync("User", content);
            }

            return RedirectToAction("UserList");
        }
    }
}
