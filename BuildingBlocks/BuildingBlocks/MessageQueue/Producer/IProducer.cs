using BuildingBlocks.MessageQueue.Requests;

namespace BuildingBlocks.MessageQueue.Producer;

public interface IProducer
{
    Task PublishMessage(PublishRequest request, CancellationToken cancellationToken);
}