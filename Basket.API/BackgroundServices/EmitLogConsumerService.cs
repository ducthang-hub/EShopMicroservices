using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.Consumer;

namespace Basket.API.BackgroundServices;

public class EmitLogConsumerService(IConsumer consumer, ILogger<EmitLogConsumerService> logger) : ConsumerService(consumer, logger)
{
    protected override string QueueName { get; set; } = "Logs";

    protected override Task HandleMessage(string message)
    {
        logger.LogInformation($" [x] Emitted log {message}");
        return Task.CompletedTask;
    }
}