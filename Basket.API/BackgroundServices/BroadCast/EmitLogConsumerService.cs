using BuildingBlocks.MessageQueue.Consumer;
using BuildingBlocks.MessageQueue.Requests;

namespace Basket.API.BackgroundServices.BroadCast;

public class EmitLogConsumerService(IConsumer consumer, ILogger<EmitLogConsumerService> logger) : ConsumerService(consumer, logger)
{
    protected override ConsumeRequest ConsumeRequest { get; set; } = new()
    {
        Exchange = "Event",
        ExchangeType = RabbitMQ.Client.ExchangeType.Fanout,
        RoutingKeys = Array.Empty<string>()
    };
    
    protected override Task HandleMessage(string message)
    {
        logger.LogInformation($" [x] Emitted log {message}");
        return Task.CompletedTask;
    }
}