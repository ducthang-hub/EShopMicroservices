using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.Consumer;

namespace Basket.API.BackgroundServices;

public class EmitLogConsumerService(IConsumer consumer, ILogger<EmitLogConsumerService> logger) : ConsumerService<EmitLogConsumerService>(consumer, logger)
{
    protected override string QueueName { get; set; } = "Logs";
}