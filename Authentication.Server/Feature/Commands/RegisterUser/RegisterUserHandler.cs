using System.Net;
using Authentication.Server.Domains;
using BuildingBlocks.Helpers;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Authentication.Server.Feature.Commands.RegisterUser;

public class RegisterUserHandler
    (
        ILogger<RegisteredWaitHandle> logger,
        UserManager<User> userManager
    ) 
    : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        var response = new RegisterUserResponse();
        
        try
        {
            var existUser = await userManager.FindByEmailAsync(payload.Email);
            if (existUser is not null)
            {
                response.Message = $"Email {payload.Email} is already occupied";
                response.Status = HttpStatusCode.Conflict;
                return response;
            }
            
            var user = new User
            {
                UserName = payload.Email,
                Email = payload.Email
            };
            var result = await userManager.CreateAsync(user, payload.Password);
            if (!result.Succeeded)
            {
                response.Message = "Cannot register right now, please try again later";
                return response;
            }
            
            response.Data = new
            {
                UserName = user.UserName,
                Password = user.PasswordHash
            };
            response.Status = HttpStatusCode.Created;
        }
        catch (Exception ex)
        {
             ex.LogError(logger);
        }

        return response;
    }
}