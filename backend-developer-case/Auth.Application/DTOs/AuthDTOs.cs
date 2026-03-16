namespace Auth.Application.DTOs
{
    // Kayıt olurken dışarıdan beklediğimiz veri
    public record RegisterRequest(string Email, string Password, string FirstName, string LastName);

    // Giriş yaparken dışarıdan beklediğimiz veri
    public record LoginRequest(string Email, string Password);

    // Başarılı girişte dışarıya döneceğimiz veri
    public record TokenResponse(string AccessToken, string RefreshToken, DateTime Expiration);
}