using Auth.API.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration _configuration;

        // appsettings.json içindeki gizli anahtarları okumak için IConfiguration kullanıyoruz
        public AuthController(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        [HttpPost("login")]
        public IActionResult Login([FromBody] LoginDto loginDto)
        {
            // Kullanıcı adı "admin", şifre "12345" ise kabul et.
            if (loginDto.Username == "admin" && loginDto.Password == "12345")
            {
                var token = GenerateJwtToken(loginDto.Username);

                return Ok(new
                {
                    Status = "Success",
                    Token = token,
                    Message = "Giriş başarılı, token üretildi!"
                });
            }

            // Bilgiler yanlışsa 401 Unauthorized (Yetkisiz) dön.
            return Unauthorized(new { Message = "Kullanıcı adı veya şifre hatalı!" });
        }

        // Token üretim mantığı (VIP Kartı Basma Makinesi)
        private string GenerateJwtToken(string username)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");
            var secretKey = Encoding.UTF8.GetBytes(jwtSettings["Secret"]!);

            // Token'ın içine koyacağımız bilgiler (Örn: Kim bu adam? Rolü ne?)
            var claims = new[]
            {
                new Claim(ClaimTypes.Name, username),
                new Claim(ClaimTypes.Role, "Admin"),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            // Token'ı oluşturma ve şifreleme işlemi
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.UtcNow.AddHours(1), // 1 saat geçerli
                signingCredentials: new SigningCredentials(new SymmetricSecurityKey(secretKey), SecurityAlgorithms.HmacSha256)
            );

            // Token'ı şifreli bir metin (string) olarak geri dön
            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}