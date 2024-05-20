using Asp.Versioning;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using MyAssistant.Core.Service.CommandsAndQueries.Authentication;
using MyAssistant.Core.Contract.DTOs.Authentication;

namespace MyAssistant.Core.Api.Controllers.Authentication;

[ApiController]
[ApiVersion(1.0)]
[Route("api/v{version:apiVersion}/[controller]")]
public class AuthenticationController(IMediator Mediator, ILogger Logger) : ControllerBase
{
    [HttpPost]
    [Route("login")]
    public async Task<IActionResult> Login([FromServices] IRequestHandler<LoginCommand, LoginResponse> loginCommand, [FromBody]LoginRequest request)
    {
        try
        {
            if(!ModelState.IsValid) return BadRequest("Invalid payload");
            LoginResponse loginResponse = await Mediator.Send(new LoginCommand() { UserName = request.UserName, Password = request.Password });
            return Ok(loginResponse);
        }
        catch (Exception ex)
        {
            Logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }



    [HttpPost]
    [Route("register")]
    public async Task<IActionResult> Register([FromServices] IRequestHandler<RegistrationCommand, RegistrationResponse> registrationCommand, [FromBody] RegistrationRequest request)
    {
        try
        {
            if(!ModelState.IsValid) return BadRequest("Invalid payload");

            RegistrationResponse user = await Mediator.Send(new RegistrationCommand
            {
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
                Email = request.Email,
                Password = request.Password,
                Role = request.Role
            });

            return CreatedAtAction(nameof(Register), user);
        }
        catch(Exception ex)
        {
            Logger.LogError(ex.Message);
            return StatusCode(StatusCodes.Status500InternalServerError, ex.Message);
        }
    }
}
