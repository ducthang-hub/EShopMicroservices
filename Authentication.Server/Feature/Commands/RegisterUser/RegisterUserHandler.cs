using System.Net;
using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using BuildingBlocks.Helpers;
using MediatR;

namespace Authentication.Server.Feature.Commands.RegisterUser;

public class RegisterUserHandler
    (
        AuthDbContext dbContext,
        ILogger<RegisteredWaitHandle> logger
    ) 
    : IRequestHandler<RegisterUserCommand, RegisterUserResponse>
{
    public async Task<RegisterUserResponse> Handle(RegisterUserCommand request, CancellationToken cancellationToken)
    {
        var payload = request.Payload;
        var response = new RegisterUserResponse();
        
        try
        {
            var user = new User
            {
                UserName = payload.UserName,
                PasswordHash = payload.Password
            };
            await dbContext.Users.AddAsync(user, cancellationToken);
            await dbContext.SaveChangesAsync(cancellationToken);

            response.Data = user;
            response.Status = HttpStatusCode.Created;
        }
        catch (Exception ex)
        {
             ex.LogError(logger);
        }

        return response;
    }
}