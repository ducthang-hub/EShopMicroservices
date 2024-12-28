using BuildingBlocks.MessageQueue.Consumer;
using BuildingBlocks.MessageQueue.Requests;

namespace Basket.API.BackgroundServices.WorkQueues;

public class AnotherMessageConsumerService(ILogger<AnotherMessageConsumerService> logger, IConsumer consumer) : ConsumerService(consumer, logger)
{
    protected override ConsumeRequest ConsumeRequest { get; set; } = new()
    {
        Exchange = "Hello",
        ExchangeType = RabbitMQ.Client.ExchangeType.Fanout,
        RoutingKeys = Array.Empty<string>()
    };

    protected override async Task HandleMessage(string message)
    {
        logger.LogInformation($" [x] Consumer by {nameof(AnotherMessageConsumerService)} Received {message}");
                
        int dots = message.Split('.').Length - 1;
        await Task.Delay(dots * 1000);

        logger.LogInformation($" [x] Consumer by {nameof(AnotherMessageConsumerService)} Done");
    }
}