using Microsoft.AspNetCore.Identity;

namespace Auth.Domain.Entities
{
    // IdentityUser'dan miras alarak Identity'nin sunduğu Id, Email, PasswordHash vb. özelliklere sahip oluyoruz.
    public class AppUser : IdentityUser
    {
        public string FirstName { get; set; } = string.Empty;
        public string LastName { get; set; } = string.Empty;
        public string? RefreshToken { get; set; }
        public DateTime? RefreshTokenExpiryTime { get; set; }
    }
}