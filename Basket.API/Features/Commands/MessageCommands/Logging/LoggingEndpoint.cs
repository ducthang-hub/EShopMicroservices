using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Features.Commands.MessageCommands.Logging;

public class LoggingEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("logging", async (IMediator mediator, [FromBody] LoggingRequest request) =>
        {
            var response = await mediator.Send(new LoggingCommand(request));
            return response;
        });
    }
}