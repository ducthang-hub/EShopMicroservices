using Carter;
using MediatR;

namespace Authentication.Server.Feature.Commands.RegisterUser;

public class RegisterUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/authen/register", async (RegisterUserRequest request, IMediator mediator) =>
        {
            var response = await mediator.Send(new RegisterUserCommand(request));
            return response;
        });
    }
}