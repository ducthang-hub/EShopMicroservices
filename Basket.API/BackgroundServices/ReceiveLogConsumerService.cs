using BuildingBlocks.MessageQueue.Consumer;

namespace Basket.API.BackgroundServices;

public class ReceiveLogConsumerService(IConsumer consumer, ILogger<ReceiveLogConsumerService> logger) : ConsumerService<ReceiveLogConsumerService>(consumer, logger)
{
    protected override string QueueName { set; get; } = "Logs";
}