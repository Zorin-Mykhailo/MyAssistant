using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyAssistant.Core.Contract.DTOs.Authentication;
using MyAssistant.Core.Service.CommandsAndQueries.Authentication;

namespace MyAssistant.Core.Api.Controllers.Authentication;


[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class TokenController(IMediator Mediator, ILogger Logger) : ControllerBase
{
    [HttpPost]
    [Route("refresh")]
    public async Task<IActionResult> RefreshToken([FromServices] IRequestHandler<TokenRefreshCommand, TokenApiResponse> refreshTokenCommand, [FromBody] TokenApiRequest request)
    {
        try
        {
            if(!ModelState.IsValid) return BadRequest("Invalid payload");
            TokenApiResponse tokenResponse = await Mediator.Send(new TokenRefreshCommand { AccessToken = request.AccessToken, RefreshToken = request.RefreshToken});
            return Ok(tokenResponse);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }



    [HttpPost, Authorize]
    [Route("revoke")]
    public async Task<IActionResult> RevokeToken([FromServices] IRequestHandler<RevokeRefreshTokenApiCommand, bool> revokeRefreshTokenCommand, [FromBody] TokenApiRequest request)
    {
        try
        {
            if(!ModelState.IsValid) return BadRequest("Invalid payload");

            bool result = await Mediator.Send(new RevokeRefreshTokenApiCommand { AccessToken = request.AccessToken, RefreshToken = request.RefreshToken });
            return Ok(result);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}