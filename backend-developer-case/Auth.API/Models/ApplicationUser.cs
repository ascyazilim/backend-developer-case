using Microsoft.AspNetCore.Identity;

namespace Auth.API.Models
{
    // IdentityUser'ı miras alarak Microsoft'un hazır özelliklerine (Email, UserName, PasswordHash vs.) sahip oluyoruz.
    public class ApplicationUser : IdentityUser
    {
        
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}