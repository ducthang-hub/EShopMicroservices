using Carter;
using MediatR;

namespace Authentication.Server.Features.Login;

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/authen/login", async (IMediator mediator, LoginRequest request, CancellationToken cancellationToken) =>
        {
            var response = await mediator.Send(new LoginCommand(request), cancellationToken);
            return response;
        });
    }
}