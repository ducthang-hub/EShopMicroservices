using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Basket.API.Features.Commands.MessageCommands.RabbitMQ;

public class MessageResponse : ErrorResponse
{
}

public class MessageCommand : ICommand<MessageResponse>
{
    
}