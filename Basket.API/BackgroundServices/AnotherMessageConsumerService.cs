using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.Consumer;

namespace Basket.API.BackgroundServices;

public class AnotherMessageConsumerService(ILogger<AnotherMessageConsumerService> logger, IConsumer consumer) : BackgroundService
{
    private const string QueueName = "HelloMotherFucker";
    
    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => ConsumeMessages(cancellationToken), cancellationToken);
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        try
        {
            await consumer.ConsumeMessages(QueueName, nameof(AnotherMessageConsumerService), cancellationToken);
            cancellationToken.WaitHandle.WaitOne();
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}