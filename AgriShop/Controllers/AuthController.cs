using AgriShop.Models;
using AgriShop.Helpers;
using AgriShop.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using JwtSettings = AgriShop.Helpers.JwtSettings;

namespace AgriShop.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly AgriShopContext _context;
        private readonly JwtSettings _jwtSettings;

        public AuthController(AgriShopContext context, IOptions<JwtSettings> jwtSettings)
        {
            _context = context;
            _jwtSettings = jwtSettings.Value;
        }


        #region Register
        // ✅ POST: api/auth/register
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDTO dto)
        {
            if (await _context.Users.AnyAsync(u => u.UserName == dto.Username))
                return BadRequest("Username already exists.");

            var user = new User
            {
                UserName = dto.Username,
                Password = BCrypt.Net.BCrypt.HashPassword(dto.Password),
                Role = "Customer"
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "Registration successful"
            });
        }
        #endregion

        #region Login

        //  POST: api/auth/login
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDTO dto)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == dto.Username);
            if (user == null || user.Password != dto.Password)
                return Unauthorized("Invalid credentials.");


            var token = GenerateJwtToken(user);

            return Ok(new
            {
                Token = token,
                Username = user.UserName,
                Role = user.Role,
                Message = "Login successful"
            });
        }

        #endregion

        #region JWT Token

        // ✅ JWT Token Generator
        private string GenerateJwtToken(User user)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtSettings.Key));

            var claims = new List<Claim>
    {
        new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()), 
        new Claim(ClaimTypes.Name, user.UserName ?? ""),              // safe null check
        new Claim(ClaimTypes.Role, user.Role ?? "")
    };

            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
                issuer: _jwtSettings.Issuer,
                audience: _jwtSettings.Audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(_jwtSettings.DurationInMinutes),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token);
        }

        #endregion
    }
}