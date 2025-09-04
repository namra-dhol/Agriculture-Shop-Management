//public class RegisterRequest
//{
//    [Required]
//    [StringLength(100)]
//    public string UserName { get; set; } = string.Empty;

//    [Required]
//    [EmailAddress]
//    [StringLength(100)]
//    public string Email { get; set; } = string.Empty;

//    [Required]
//    [StringLength(100, MinimumLength = 6)]
//    public string Password { get; set; } = string.Empty;

//    [Required]
//    [StringLength(200)]
//    public string Address { get; set; } = string.Empty;

//    [Required]
//    [StringLength(20)]
//    public string Phone { get; set; } = string.Empty;

//    [Required]
//    [RegularExpression("^(Admin|Customer)$", ErrorMessage = "Role must be either 'Admin' or 'Customer'")]
//    public string Role { get; set; } = "Customer";
//}

//public class LoginRequest
//{
//    [Required]
//    public string UserName { get; set; } = string.Empty;

//    [Required]
//    public string Password { get; set; } = string.Empty;
//}

//public class AuthResponse
//{
//    public bool Success { get; set; }
//    public string Token { get; set; } = string.Empty;
//    public string Message { get; set; } = string.Empty;
//    public UserInfo? User { get; set; }
//}

//public class UserInfo
//{
//    public int UserId { get; set; }
//    public string UserName { get; set; } = string.Empty;
//    public string Email { get; set; } = string.Empty;
//    public string Role { get; set; } = string.Empty;
//    public string Address { get; set; } = string.Empty;
//    public string Phone { get; set; } = string.Empty;
//}