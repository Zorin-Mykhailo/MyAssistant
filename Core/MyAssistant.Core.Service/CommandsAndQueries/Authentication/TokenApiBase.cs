using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace MyAssistant.Core.Service.CommandsAndQueries.Authentication;

public abstract class TokenApiBase
{
    protected readonly IConfiguration _configuration;

    public TokenApiBase(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateAccessToken(IEnumerable<Claim> claims)
    {
        SymmetricSecurityKey authSigningKey = new (Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]!));
        long tokenExpiryTimeInMinutes = Convert.ToInt64(_configuration["JWTKey:TokenExpiryTimeInMinutes"]);
        SecurityTokenDescriptor tokenDescriptor = new ()
        {
            Issuer = _configuration["JWTKey:ValidIssuer"],
            Audience = _configuration["JWTKey:ValidAudience"],
            Expires = DateTime.UtcNow.AddMinutes(tokenExpiryTimeInMinutes),
            SigningCredentials = new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256),
            Subject = new ClaimsIdentity(claims)
        };
        JwtSecurityTokenHandler tokenHandler = new ();
        SecurityToken token = tokenHandler.CreateToken(tokenDescriptor);

        return tokenHandler.WriteToken(token);
    }

    
    
    public static string GenerateRefreshToken()
    {
        return Convert.ToBase64String(Encoding.UTF8.GetBytes(Guid.NewGuid().ToString()));
    }



    public ClaimsPrincipal GetPrincipalFromExpiredToken(string  token)
    {
        TokenValidationParameters tokenValidationParameters = new()
        {
            ValidateAudience = false, //you might want to validate the audience and issuer depending on your use case
            ValidateIssuer = false,
            ValidateIssuerSigningKey = true,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWTKey:Secret"]!)),
            ValidateLifetime = true //here we are saying that we don't care about the token's expiration date
        };
        JwtSecurityTokenHandler tokenHandler = new ();
        ClaimsPrincipal principal = tokenHandler.ValidateToken(token, tokenValidationParameters, out SecurityToken securityToken);
        JwtSecurityToken? jwtSecurityToken = securityToken as JwtSecurityToken;
        return jwtSecurityToken == null || !jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256, StringComparison.InvariantCultureIgnoreCase)
            ? throw new SecurityTokenException("Invalid token")
            : principal;
    }
}