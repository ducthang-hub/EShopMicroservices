using Carter;
using MediatR;

namespace Authentication.Server.Feature.Commands.GenerateAccessToken;

public class GenerateTokenEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/connect/token", async (HttpContext httpContext, IMediator mediator) =>
        {
        });
    }
}