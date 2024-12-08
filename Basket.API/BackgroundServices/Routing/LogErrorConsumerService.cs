using BuildingBlocks.Logging.Enums;
using BuildingBlocks.MessageQueue.Consumer;
using BuildingBlocks.MessageQueue.Requests;

namespace Basket.API.BackgroundServices.Routing;

public class LogErrorConsumerService(IConsumer consumer, ILogger<LogErrorConsumerService> logger) : ConsumerService(consumer, logger)
{
    protected override ConsumeRequest ConsumeRequest { get; set; } = new()
    {
        Exchange = "Monitoring",
        ExchangeType = RabbitMQ.Client.ExchangeType.Direct,
        RoutingKeys = new[]
        {
            LoggingSeverityEnum.Error.ToString(),
        }
    };

    protected override Task HandleMessage(string message)
    {
        logger.LogInformation($" [x] Log {message}");
        return Task.CompletedTask;
    }
}