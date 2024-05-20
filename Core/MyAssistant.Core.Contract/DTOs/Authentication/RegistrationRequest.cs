using System.ComponentModel.DataAnnotations;

namespace MyAssistant.Core.Contract.DTOs.Authentication;

public record RegistrationRequest
{
    [Required(ErrorMessage = "User Name is required")]
    public string UserName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Name is required")]
    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    [EmailAddress]
    [Required(ErrorMessage = "Email is required")]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "Password is required")]
    public string Password { get; set; } = string.Empty;

    [Required(ErrorMessage = "Role is required")]
    public string Role { get; set; } = string.Empty;
}