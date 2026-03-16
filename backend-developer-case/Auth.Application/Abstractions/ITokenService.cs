using System.Security.Claims;

namespace Auth.Application.Abstractions
{
    public interface ITokenService
    {
        // JWT Access Token üretir
        string GenerateAccessToken(IEnumerable<Claim> claims);

        // Uzun ömürlü Refresh Token üretir
        string GenerateRefreshToken();

        // Refresh token kullanımı için süresi dolmuş token'dan kullanıcı bilgilerini okur
        ClaimsPrincipal? GetPrincipalFromExpiredToken(string token);
    }
}