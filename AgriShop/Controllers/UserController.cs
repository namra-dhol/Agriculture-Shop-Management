using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;
using FluentValidation;

namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AgriShopContext context;
        private readonly IValidator<User> validator;

        public UserController(AgriShopContext context, IValidator<User> validator)
        {
            this.context = context;
            this.validator = validator;
        }

        #region GetAllUsers
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            var users = await context.Users.ToListAsync();
            return Ok(users);
        }
        #endregion

        #region GetUserById
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUserById(int id)
        {
            var user = await context.Users.FindAsync(id);
            if (user == null)
                return NotFound();

            return Ok(user);
        }
        #endregion

        #region InsertUser
        [HttpPost]
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
                // You can hash the password here if needed
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
            existingUser.Password = user.Password;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            existingUser.Role = user.Role;

            try
            {
                context.Users.Update(existingUser);
                await context.SaveChangesAsync();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An" +
                    $" error occurred while updating the user: {ex.Message}");
            }
        }
        #endregion

        #region DeleteUserById
        [HttpDelete("{id}")]
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
        public async Task<ActionResult<IEnumerable<User>>> GetTopNUsers([FromQuery] int n = 2)
        {
            var users = await context.Users.Take(n).ToListAsync();
            return Ok(users);
        }
        #endregion
    }
}
