using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyAssistant.Core.Data.Entities.Auth;
using System.Security.Claims;

namespace MyAssistant.Core.Service.CommandsAndQueries.Authentication;

public record RevokeRefreshTokenApiCommand : IRequest<bool>
{
    public string AccessToken {  get; set; } = string.Empty;

    public string RefreshToken { get; set; } = string.Empty;

    public class Handler : TokenApiBase, IRequestHandler<RevokeRefreshTokenApiCommand, bool>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager, IConfiguration configuration) : base(configuration)
        {
            _userManager = userManager;
        }

        public async Task<bool> Handle(RevokeRefreshTokenApiCommand command, CancellationToken cancellationToken = default)
        {
            ClaimsPrincipal principal = GetPrincipalFromExpiredToken(command.AccessToken);
            string? userName = principal.Identity.Name; //this is mapped to the Name claim by default

            AppUser user = await _userManager.FindByNameAsync(userName) ?? throw new Exception("Invalid username");

            user.RefreshToken = "⛔ " + Guid.NewGuid().ToString();
            user.RefreshTokenExpiryTime = DateTime.UtcNow;

            await _userManager.UpdateAsync(user);

            return true;
        }
    }
}