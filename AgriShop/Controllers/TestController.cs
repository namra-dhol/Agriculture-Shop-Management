using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestController : ControllerBase
    {
        [HttpGet("public")]
        public IActionResult PublicEndpoint()
        {
            return Ok(new { message = "This is a public endpoint - no authentication required" });
        }

        [HttpGet("protected")]
        [Authorize]
        public IActionResult ProtectedEndpoint()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;
            var userRole = User.FindFirst(ClaimTypes.Role)?.Value;

            return Ok(new 
            { 
                message = "This is a protected endpoint - authentication required",
                userId = userId,
                userName = userName,
                userRole = userRole
            });
        }

        [HttpGet("admin")]
        [Authorize(Roles = "Admin")]
        public IActionResult AdminEndpoint()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new 
            { 
                message = "This is an admin-only endpoint",
                userId = userId,
                userName = userName
            });
        }

        [HttpGet("customer")]
        [Authorize(Roles = "Customer")]
        public IActionResult CustomerEndpoint()
        {
            var userId = User.FindFirst("UserId")?.Value;
            var userName = User.FindFirst(ClaimTypes.Name)?.Value;

            return Ok(new 
            { 
                message = "This is a customer-only endpoint",
                userId = userId,
                userName = userName
            });
        }
    }
} 