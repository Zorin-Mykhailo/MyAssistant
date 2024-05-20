using MediatR;
using Microsoft.AspNetCore.Identity;
using MyAssistant.Core.Contract.DTOs.Authentication;
using MyAssistant.Core.Data.Entities.Auth;

namespace MyAssistant.Core.Service.CommandsAndQueries.Authentication;

public record RegistrationCommand : IRequest<RegistrationResponse>
{
    public string UserName { get; set; } = string.Empty;

    public string FirstName { get; set; } = string.Empty;

    public string LastName { get; set; } = string.Empty;

    public string Email { get; set; } = string.Empty;

    public string Password { get; set; } = string.Empty;

    public string Role { get; set; } = string.Empty;



    public class Handler(UserManager<AppUser> UserManager, RoleManager<IdentityRole> RoleManager) : IRequestHandler<RegistrationCommand, RegistrationResponse>
    {
        public async Task<RegistrationResponse> Handle(RegistrationCommand request, CancellationToken cancellation = default)
        {
            AppUser? userExist = await UserManager.FindByEmailAsync(request.UserName);
            if(userExist != null) throw new Exception("User already exists");

            AppUser user = new ()
            {
                Email = request.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = request.UserName,
                FirstName = request.FirstName,
                LastName = request.LastName,
            };
            IdentityResult createdUserResult = await UserManager.CreateAsync(user, request.Password);

            if(!createdUserResult.Succeeded) throw new Exception("User creation failed! Please check user details and try again.");

            if(!await RoleManager.RoleExistsAsync(request.Role))
                await RoleManager.CreateAsync(new IdentityRole(request.Role));

            if(await RoleManager.RoleExistsAsync(request.Role))
                await UserManager.AddToRoleAsync(user, request.Role);

            AppUser registeredUser = (await UserManager.FindByNameAsync(request.UserName))!;

            return new RegistrationResponse
            {
                UserId = registeredUser.Id,
                UserName = registeredUser.UserName!,
                FirstName = registeredUser.FirstName,
                LastName = registeredUser.LastName,
                Email = registeredUser.Email!,
                Role = request.Role
            };
        }
    }
}