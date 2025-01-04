using BuildingBlocks.CQRS;
using BuildingBlocks.Helpers;

namespace Authentication.Server.Feature.Commands.AuthCommands.Login;

public class LoginHandler
    (
        IServiceProvider serviceProvider,
        ILogger<LoginHandler> logger
    )
    : ICommandHandler<LoginCommand, LoginResponse>
{
    public async Task<LoginResponse> Handle(LoginCommand request, CancellationToken cancellationToken)
    {
        var response = new LoginResponse();
        var payload = request.Payload;
        try
        {
            
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
        return response;
    }
}