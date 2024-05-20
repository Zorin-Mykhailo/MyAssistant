using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using MyAssistant.Core.Contract.DTOs.Authentication;
using MyAssistant.Core.Data.Entities.Auth;
using System.Security.Claims;
using System.IdentityModel.Tokens.Jwt;

namespace MyAssistant.Core.Service.CommandsAndQueries.Authentication;

public record LoginCommand : IRequest<LoginResponse>
{
    public string UserName { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;



    public class Handler : TokenApiBase, IRequestHandler<LoginCommand, LoginResponse>
    {
        private readonly UserManager<AppUser> _userManager;

        public Handler(UserManager<AppUser> userManager, IConfiguration configuration) : base(configuration)
        {
            _userManager = userManager;
        }

        public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken = default)
        {
            AppUser user = await _userManager.FindByNameAsync(request.UserName) ?? throw new Exception("Invalid username");

            if(!await _userManager.CheckPasswordAsync(user, request.Password)) throw new Exception("Invalid password");

            IList<string> userRoles = await _userManager.GetRolesAsync(user);
            List<Claim> authClaims = [new (ClaimTypes.Name, user.UserName!), new (JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())];
            authClaims.AddRange(userRoles.Select(userRole => new Claim(ClaimTypes.Role, userRole)));

            string accessToken = GenerateAccessToken(authClaims);
            string refreshToken = GenerateRefreshToken();

            user.RefreshToken = refreshToken;
            user.RefreshTokenExpiryTime = DateTime.UtcNow.AddMinutes(Convert.ToInt16(_configuration["JWTKey:RefreshTokenExpiryTimeInMinutes"]));

            await _userManager.UpdateAsync(user);

            return new LoginResponse
            {
                UserId = user.Id,
                UserName = user.UserName!,
                TokenApiResponse = new TokenApiResponse
                {
                    AccessToken = accessToken,
                    RefreshToken = refreshToken,
                }
            };
        }
    }
}