namespace MyAssistant.Core.Contract.DTOs.Authentication;

public record RegistrationResponse
{
    public string UserId { get; set; } = string.Empty;

    public string UserName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;
}