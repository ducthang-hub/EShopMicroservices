using Carter;
using MediatR;

namespace Authentication.Server.Feature.Commands.AuthCommands.Login;

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/login", async (IMediator mediator, LoginRequest request) =>
        {
            var response = await mediator.Send(new LoginCommand(request));
            return response;
        });
    }
}