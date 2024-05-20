namespace MyAssistant.Core.Contract.DTOs.Authentication;

public record TokenApiResponse
{
    public string AccessToken { get; set; } = string.Empty;
    public string RefreshToken { get; set; } = string.Empty;
}