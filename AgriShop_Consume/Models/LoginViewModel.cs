using System.ComponentModel.DataAnnotations;

namespace AgriShop_Consume.Models
{

    public class LoginResponse
    {
        public string Token { get; set; }
        public string Role { get; set; }
        public string Email { get; set; }
        public string Message { get; set; }

        public string Username { get; set; }
    }
    public class LoginViewModel
    {
        
      
        public string? Email { get; set; }

        [Required]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Username { get; set; }

    }

    public class RegisterViewModel
    {
        [Required]
        public string Name { get; set; }

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [Required, Compare("Password", ErrorMessage = "Passwords do not match.")]
        public string ConfirmPassword { get; set; }
    }

    public class RegisterDTO
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
    }


}