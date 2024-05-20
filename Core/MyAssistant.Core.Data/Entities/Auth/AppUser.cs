using Microsoft.AspNetCore.Identity;

namespace MyAssistant.Core.Data.Entities.Auth;

public class AppUser : IdentityUser
{
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public DateTime RefreshTokenExpiryTime { get; set; }
}