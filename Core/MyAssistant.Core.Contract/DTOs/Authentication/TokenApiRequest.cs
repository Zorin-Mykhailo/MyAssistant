using System.ComponentModel.DataAnnotations;

namespace MyAssistant.Core.Contract.DTOs.Authentication;

public record TokenApiRequest
{
    [Required(ErrorMessage = "Access Token is required")]
    public string AccessToken { get; set; } = string.Empty;

    [Required(ErrorMessage = "Refresh Token is required")]
    public string RefreshToken { get; set; } = string.Empty;
}