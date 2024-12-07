using Carter;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Basket.API.Features.Commands.MessageCommands.RabbitMQ;

public class MessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("message/send", async (IMediator mediator, [FromBody] string message) =>
        {
            var response = await mediator.Send(new MessageCommand(message));
            return response;
        });
    }
}