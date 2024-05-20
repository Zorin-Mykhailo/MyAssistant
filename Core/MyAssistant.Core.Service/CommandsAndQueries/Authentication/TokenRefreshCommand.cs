using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyAssistant.Core.Contract.DTOs.Authentication;
using MyAssistant.Core.Data.Entities.Auth;
using System.Security.Claims;

namespace MyAssistant.Core.Service.CommandsAndQueries.Authentication;

public record TokenRefreshCommand : IRequest<TokenApiResponse>
{
    public string AccessToken { get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;


    public class Handler : TokenApiBase, IRequestHandler<TokenRefreshCommand, TokenApiResponse>
    {
        private readonly UserManager<AppUser> _userMagager;

        public Handler(UserManager<AppUser> userMagager, IConfiguration configuration) : base(configuration)
        {
            _userMagager = userMagager;
        }

        public async Task<TokenApiResponse> Handle(TokenRefreshCommand request, CancellationToken cancellationToken = default)
        {
            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(request.AccessToken);
            string? username = principal.Identity.Name; //this is mapped to the Name claim by default
            AppUser? user = await _userMagager.FindByNameAsync(username) ?? throw new Exception("Invalid username");

            if(user is null || user.RefreshToken != request.RefreshToken || user.RefreshTokenExpiryTime <= DateTime.UtcNow)
                throw new Exception("Invalid client request");

            string newAccessToken = GenerateAccessToken(principal.Claims);
            string newRefreshToken = GenerateRefreshToken();

            user.RefreshToken = newRefreshToken;

            await _userMagager.UpdateAsync(user);

            return new TokenApiResponse { AccessToken = newAccessToken, RefreshToken = newRefreshToken };
        }
    }
}