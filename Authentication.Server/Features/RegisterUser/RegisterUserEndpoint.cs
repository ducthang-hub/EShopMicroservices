using Carter;
using MediatR;

namespace Authentication.Server.Features.RegisterUser;

public class RegisterUserEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/auth/register", async (RegisterUserRequest request, IMediator mediator) =>
        {
            var response = await mediator.Send(new RegisterUserCommand(request));
            return response;
        });
    }
}