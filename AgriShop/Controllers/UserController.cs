using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using BCrypt.Net;
using System.Security.Claims;


namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    //[Authorize] // Require authentication for all endpoints
    public class UserController : ControllerBase
    {
        private readonly AgriShopContext context;
        private readonly IValidator<User> validator;

        public UserController(AgriShopContext context, IValidator<User> validator)
        {
            this.context = context;
            this.validator = validator;
        }

        /*#region GetAllUsers
        [HttpGet]
        //[Authorize(Roles = "Admin")] // Only admins can see all users
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await context.Users.ToListAsync();
            return Ok(users);
        }
        #endregion*/

        #region User List with Pagination

        [HttpGet]
        public async Task<IActionResult> GetUser(int pageNumber = 1, int pageSize = 5)
        {
            try
            {
                var totalRecords = await context.Users.CountAsync();

                var users = await context.Users
                    .OrderBy(u => u.UserId)   // Order by Id to maintain consistent paging
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                var response = new
                {
                    TotalRecords = totalRecords,
                    PageSize = pageSize,
                    CurrentPage = pageNumber,
                    TotalPages = (int)Math.Ceiling((double)totalRecords / pageSize),
                    Users = users
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        #endregion

        #region GetUserById
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            // Users can only see their own profile, admins can see any profile
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (currentUserRole != "Admin" && currentUserId != id)
            {
                return Forbid();
            }

            var user = await context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        #endregion

        #region InsertUser
        [HttpPost]
        //[Authorize(Roles = "Admin")] // Only admins can create users
        public async Task<IActionResult> InsertUser([FromBody] User user)
        {
            var validationResult = await validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }
            try
            {
                // Hash the password before saving
                if (!string.IsNullOrEmpty(user.Password))
                {
                    user.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
                }

                context.Users.Add(user);
                await context.SaveChangesAsync();
                return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while saving the user: {ex.Message}");
            }
        }
        #endregion

        #region UpdateUserById
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUser(int id, [FromBody] User user)
        {
            // Users can only update their own profile, admins can update any profile
            var currentUserId = int.Parse(User.FindFirst("UserId")?.Value ?? "0");
            var currentUserRole = User.FindFirst(ClaimTypes.Role)?.Value ?? "";

            if (currentUserRole != "Admin" && currentUserId != id)
            {
                return Forbid();
            }

            if (id != user.UserId)
                return BadRequest();

            var validationResult = await validator.ValidateAsync(user);
            if (!validationResult.IsValid)
            {
                return BadRequest(validationResult.Errors.Select(e => new
                {
                    Property = e.PropertyName,
                    Error = e.ErrorMessage
                }));
            }

            var existingUser = await context.Users.FindAsync(id);
            if (existingUser == null)
                return NotFound();

            // Update fields
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            
            // Only admins can change roles
            if (currentUserRole == "Admin")
            {
                existingUser.Role = user.Role;
            }

            // Hash password if it's being updated
            if (!string.IsNullOrEmpty(user.Password) && user.Password != existingUser.Password)
            {
                existingUser.Password = BCrypt.Net.BCrypt.HashPassword(user.Password);
            }

            try
            {
                context.Users.Update(existingUser);
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred while updating the user: {ex.Message}");
            }
        }
        #endregion

        #region DeleteUserById
        [HttpDelete("{id}")]
        //[Authorize(Roles = "Admin")] // Only admins can delete users
        public IActionResult DeleteUser(int id)
        {
            var user = context.Users.Find(id);
            if (user == null)
                return NotFound();

            context.Users.Remove(user);
            context.SaveChanges();
            return NoContent();
        }
        #endregion

        #region FilterUsers
        [HttpGet("Filter")]
        //[Authorize(Roles = "Admin")] // Only admins can filter users
        public async Task<ActionResult<IEnumerable<User>>> FilterUsers(
            [FromQuery] string? userName,
            [FromQuery] string? role)
        {
            var query = context.Users.AsQueryable();

            if (!string.IsNullOrEmpty(userName))
                query = query.Where(u => u.UserName!.Contains(userName));

            if (!string.IsNullOrEmpty(role))
                query = query.Where(u => u.Role!.Contains(role));

            return await query.ToListAsync();
        }
        #endregion

        #region GetTopNUsers
        [HttpGet("top")]
        //[Authorize(Roles = "Admin")] // Only admins can get top users
        public async Task<ActionResult<IEnumerable<User>>> GetTopNUsers([FromQuery] int n = 2)
        {
            var users = await context.Users.Take(n).ToListAsync();
            return Ok(users);
        }
        #endregion
    }
}
