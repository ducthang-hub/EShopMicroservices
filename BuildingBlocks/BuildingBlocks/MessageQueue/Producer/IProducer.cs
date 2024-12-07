namespace BuildingBlocks.MessageQueue.Producer;

public interface IProducer
{
    Task PublishMessage(string queue, string message, CancellationToken cancellationToken);
}