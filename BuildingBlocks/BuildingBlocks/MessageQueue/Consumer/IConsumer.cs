using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.Consumer;

public interface IConsumer
{
    Task ConsumeMessages(string queue, string consumedBy, CancellationToken cancellationToken);
}