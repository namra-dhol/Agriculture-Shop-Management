//using AgriShop.Models;
//using AgriShop.Services;
//using Microsoft.AspNetCore.Identity.Data;
//using Microsoft.AspNetCore.Mvc;
//using System.ComponentModel.DataAnnotations;

//public class AuthController : ControllerBase
//{
//    private readonly AgriShopContext _context;
//    private readonly IJwtService _jwtService;

//    public AuthController(AgriShopContext context, IJwtService jwtService)
//    {
//        _context = context;
//        _jwtService = jwtService;
//    }

//    [HttpPost("register")]
//    public async Task<ActionResult<AuthResponse>> Register([FromBody] RegisterRequest request)
//    {
//        try
//        {
//            // Validate the request
//            var validationResults = new List<ValidationResult>();
//            var validationContext = new ValidationContext(request);
//            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
//            {
//                return BadRequest(new AuthResponse
//                {
//                    Success = false,
//                    Message = "Validation failed: " + string.Join(", ", validationResults.Select(v => v.ErrorMessage))
//                });
//            }

//            // Check if username already exists
//            if (await _context.Users.AnyAsync(u => u.UserName == request.UserName))
//            {
//                return BadRequest(new AuthResponse
//                {
//                    Success = false,
//                    Message = "Username already exists"
//                });
//            }

//            // Check if email already exists
//            if (await _context.Users.AnyAsync(u => u.Email == request.Email))
//            {
//                return BadRequest(new AuthResponse
//                {
//                    Success = false,
//                    Message = "Email already exists"
//                });
//            }

//            // Hash the password
//            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(request.Password);

//            // Create new user
//            var user = new User
//            {
//                UserName = request.UserName,
//                Email = request.Email,
//                Password = hashedPassword,
//                Address = request.Address,
//                Phone = request.Phone,
//                Role = request.Role
//            };

//            _context.Users.Add(user);
//            await _context.SaveChangesAsync();

//            // Generate JWT token
//            var token = _jwtService.GenerateToken(user);

//            return Ok(new AuthResponse
//            {
//                Success = true,
//                Token = token,
//                Message = "Registration successful",
//                User = new UserInfo
//                {
//                    UserId = user.UserId,
//                    UserName = user.UserName!,
//                    Email = user.Email!,
//                    Role = user.Role!,
//                    Address = user.Address!,
//                    Phone = user.Phone!
//                }
//            });
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, new AuthResponse
//            {
//                Success = false,
//                Message = $"An error occurred during registration: {ex.Message}"
//            });
//        }
//    }

//    [HttpPost("login")]
//    public async Task<ActionResult<AuthResponse>> Login([FromBody] LoginRequest request)
//    {
//        try
//        {
//            // Validate the request
//            var validationResults = new List<ValidationResult>();
//            var validationContext = new ValidationContext(request);
//            if (!Validator.TryValidateObject(request, validationContext, validationResults, true))
//            {
//                return BadRequest(new AuthResponse
//                {
//                    Success = false,
//                    Message = "Validation failed: " + string.Join(", ", validationResults.Select(v => v.ErrorMessage))
//                });
//            }

//            // Find user by username
//            var user = await _context.Users.FirstOrDefaultAsync(u => u.UserName == request.UserName);
//            if (user == null)
//            {
//                return Unauthorized(new AuthResponse
//                {
//                    Success = false,
//                    Message = "Invalid username or password"
//                });
//            }

//            // Verify password
//            if (!BCrypt.Net.BCrypt.Verify(request.Password, user.Password))
//            {
//                return Unauthorized(new AuthResponse
//                {
//                    Success = false,
//                    Message = "Invalid username or password"
//                });
//            }

//            // Generate JWT token
//            var token = _jwtService.GenerateToken(user);

//            return Ok(new AuthResponse
//            {
//                Success = true,
//                Token = token,
//                Message = "Login successful",
//                User = new UserInfo
//                {
//                    UserId = user.UserId,
//                    UserName = user.UserName!,
//                    Email = user.Email!,
//                    Role = user.Role!,
//                    Address = user.Address!,
//                    Phone = user.Phone!
//                }
//            });
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, new AuthResponse
//            {
//                Success = false,
//                Message = $"An error occurred during login: {ex.Message}"
//            });
//        }
//    }

//    [HttpGet("me")]
//    public async Task<ActionResult<UserInfo>> GetCurrentUser()
//    {
//        try
//        {
//            var userIdClaim = User.FindFirst("UserId");
//            if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
//            {
//                return Unauthorized();
//            }

//            var user = await _context.Users.FindAsync(userId);
//            if (user == null)
//            {
//                return NotFound();
//            }

//            return Ok(new UserInfo
//            {
//                UserId = user.UserId,
//                UserName = user.UserName!,
//                Email = user.Email!,
//                Role = user.Role!,
//                Address = user.Address!,
//                Phone = user.Phone!
//            });
//        }
//        catch (Exception ex)
//        {
//            return StatusCode(500, $"An error occurred: {ex.Message}");
//        }
//    }
//}