using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AgriShop.Models;

namespace AgriShop.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly AgriShopContext context;

        public UserController(AgriShopContext context)
        {
            this.context = context;
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
        public IActionResult InsertUser(User user)
        {
            user.Password = user.Password; // You can hash the password here if needed
            context.Users.Add(user);
            context.SaveChanges();

            return CreatedAtAction(nameof(GetUserById), new { id = user.UserId }, user);
        }
        #endregion

        #region UpdateUserById
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            if (id != user.UserId)
                return BadRequest();

            var existingUser = context.Users.Find(id);
            if (existingUser == null)
                return NotFound();

            // Update fields
            existingUser.UserName = user.UserName;
            existingUser.Email = user.Email;
            existingUser.Password = user.Password;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            existingUser.Role = user.Role;

            context.Users.Update(existingUser);
            context.SaveChanges();
            return NoContent();
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
