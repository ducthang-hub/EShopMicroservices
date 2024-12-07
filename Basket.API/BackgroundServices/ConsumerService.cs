using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.Consumer;

namespace Basket.API.BackgroundServices;

public abstract class ConsumerService<T>(IConsumer consumer, ILogger<T> logger) : BackgroundService
{
    protected virtual string QueueName { get; set; }

    protected override Task ExecuteAsync(CancellationToken cancellationToken)
    {
        return Task.Run(() => ConsumeMessages(cancellationToken), cancellationToken);
    }

    private async Task ConsumeMessages(CancellationToken cancellationToken)
    {
        try
        {
            await consumer.ConsumeMessages(QueueName, typeof(T).ToString(), cancellationToken);
            cancellationToken.WaitHandle.WaitOne();
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}