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
        [Display(Name = "Username")]
        public string Username { get; set; }  // ✅ Changed from Name to Username

        [Required, EmailAddress]
        public string Email { get; set; }

        [Required, MinLength(6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [Required]
        public string Address { get; set; }

        [Required]
        [Phone]
        public string Phone { get; set; }
    }

    public class RegisterDTO
    {
        public string Username { get; set; }  // ✅ Changed from Name to Username to match API
        public string Email { get; set; }
        public string Password { get; set; }
        public string Phone { get; set; }
        public string Address { get; set; }   // ✅ Added Address field
    }





}