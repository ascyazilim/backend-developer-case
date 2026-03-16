using Auth.Application.Abstractions;
using Auth.Application.DTOs;
using Auth.Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Auth.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ITokenService _tokenService;

        // Dependency Injection ile gerekli servisleri alıyoruz
        public AuthController(UserManager<AppUser> userManager, ITokenService tokenService)
        {
            _userManager = userManager;
            _tokenService = tokenService;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequest request)
        {
            // Kullanıcı var mı kontrolü
            var userExists = await _userManager.FindByEmailAsync(request.Email);
            if (userExists != null)
                return BadRequest("Bu e-posta adresi zaten kullanımda.");

            AppUser user = new()
            {
                Email = request.Email,
                UserName = request.Email, // Identity username'i zorunlu tutar, email'i username yapıyoruz
                FirstName = request.FirstName,
                LastName = request.LastName
            };

            // Identity'nin kendi metoduyla şifreyi hash'leyerek kaydediyoruz
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, result.Errors);

            return Ok(new { Status = "Success", Message = "Kullanıcı başarıyla oluşturuldu." });
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _userManager.FindByEmailAsync(request.Email);

            // Kullanıcı var mı ve şifre doğru mu?
            if (user != null && await _userManager.CheckPasswordAsync(user, request.Password))
            {
                // Token içine gömülecek kimlik bilgileri (Claims)
                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.UserName!),
                    new Claim(ClaimTypes.NameIdentifier, user.Id),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()), // Token'a özel benzersiz ID
                };

                // TokenService üzerinden token'ları üretiyoruz
                var accessToken = _tokenService.GenerateAccessToken(authClaims);
                var refreshToken = _tokenService.GenerateRefreshToken();

                // Refresh token mekanizmasını yönetmek için veritabanına kaydediyoruz
                user.RefreshToken = refreshToken;
                user.RefreshTokenExpiryTime = DateTime.UtcNow.AddDays(7);

                await _userManager.UpdateAsync(user);

                return Ok(new TokenResponse(
                    AccessToken: accessToken,
                    RefreshToken: refreshToken,
                    Expiration: DateTime.UtcNow.AddMinutes(60)
                ));
            }
            return Unauthorized("Geçersiz e-posta veya şifre.");
        }
    }
}