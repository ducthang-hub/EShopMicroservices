using Carter;
using MediatR;

namespace Basket.API.Features.Commands.MessageCommands.RabbitMQ;

public class MessageEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("message/send", async (IMediator mediator) =>
        {
            var response = await mediator.Send(new MessageCommand());
            return response;
        });
    }
}