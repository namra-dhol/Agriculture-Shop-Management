using System.ComponentModel.DataAnnotations;

namespace AgriShop.DTOs
{
    public class RegisterDTO
    {
        [Required]
        public string Username { get; set; }

        [Required, MinLength(6)]
        public string Password { get; set; }

        [EmailAddress]
        public string? Email { get; set; }

        [Phone]
        public string? Phone { get; set; }

        [Required]
        public string Address { get; set; } // ✅ add this
    }


}
