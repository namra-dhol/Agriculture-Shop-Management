//using Microsoft.IdentityModel.Tokens;
//using System.IdentityModel.Tokens.Jwt;
//using System.Security.Claims;
//using System.Text;
//using AgriShop.Models;

//namespace AgriShop.Services
//{
//    public interface IJwtService
//    {
//        string GenerateToken(User user);
//        ClaimsPrincipal? ValidateToken(string token);
//    }

//    public class JwtService : IJwtService
//    {
//        private readonly JwtSettings _jwtSettings;

//        public JwtService(JwtSettings jwtSettings)
//        {
//            _jwtSettings = jwtSettings;
//        }

//        public string GenerateToken(User user)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

//            var claims = new List<Claim>
//            {
//                new Claim(ClaimTypes.NameIdentifier, user.UserId.ToString()),
//                new Claim(ClaimTypes.Name, user.UserName ?? ""),
//                new Claim(ClaimTypes.Email, user.Email ?? ""),
//                new Claim(ClaimTypes.Role, user.Role ?? "Customer"),
//                new Claim("UserId", user.UserId.ToString())
//            };

//            var tokenDescriptor = new SecurityTokenDescriptor
//            {
//                Subject = new ClaimsIdentity(claims),
//                Expires = DateTime.UtcNow.AddMinutes(_jwtSettings.ExpirationInMinutes),
//                Issuer = _jwtSettings.Issuer,
//                Audience = _jwtSettings.Audience,
//                SigningCredentials = new SigningCredentials(
//                    new SymmetricSecurityKey(key),
//                    SecurityAlgorithms.HmacSha256Signature
//                )
//            };

//            var token = tokenHandler.CreateToken(tokenDescriptor);
//            return tokenHandler.WriteToken(token);
//        }

//        public ClaimsPrincipal? ValidateToken(string token)
//        {
//            var tokenHandler = new JwtSecurityTokenHandler();
//            var key = Encoding.ASCII.GetBytes(_jwtSettings.SecretKey);

//            try
//            {
//                var principal = tokenHandler.ValidateToken(token, new TokenValidationParameters
//                {
//                    ValidateIssuerSigningKey = true,
//                    IssuerSigningKey = new SymmetricSecurityKey(key),
//                    ValidateIssuer = true,
//                    ValidIssuer = _jwtSettings.Issuer,
//                    ValidateAudience = true,
//                    ValidAudience = _jwtSettings.Audience,
//                    ValidateLifetime = true,
//                    ClockSkew = TimeSpan.Zero
//                }, out SecurityToken validatedToken);

//                return principal;
//            }
//            catch
//            {
//                return null;
//            }
//        }
//    }
//} 