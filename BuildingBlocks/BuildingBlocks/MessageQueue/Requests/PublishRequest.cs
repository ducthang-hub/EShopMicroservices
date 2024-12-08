using RabbitMQ.Client;

namespace BuildingBlocks.MessageQueue.Requests;

public class PublishRequest
{
    private string _exchange;
    private string _routingKey;
    public string Exchange
    {
        get => _exchange;
        set => _exchange = value.ToLowerInvariant();
    }
    public string ExchangeType { get; set; }
    public string Message { get; set; }

    public string RoutingKey
    {
        get => _routingKey;
        set => _routingKey = value.ToLowerInvariant();
    }
}