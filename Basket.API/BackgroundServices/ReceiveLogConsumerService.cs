using BuildingBlocks.MessageQueue.Consumer;

namespace Basket.API.BackgroundServices;

public class ReceiveLogConsumerService(IConsumer consumer, ILogger<ReceiveLogConsumerService> logger) : ConsumerService(consumer, logger)
{
    protected override string QueueName { set; get; } = "Logs";
    protected override Task HandleMessage(string message)
    {
        logger.LogInformation($" [x] Printed log {message}");
        return Task.CompletedTask;
    }
}