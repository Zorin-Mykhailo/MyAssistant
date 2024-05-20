using System.ComponentModel.DataAnnotations;

namespace MyAssistant.Core.Contract.DTOs.Authentication;

public record class LoginRequest
{
    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;
}
