using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Client.Features.Commands.Login;

public class LoginEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/login",
            async (LoginRequest request, IMediator mediator) =>
            {
                var response = await mediator.Send(new LoginCommand(request));
                return response;
            });
    }
}