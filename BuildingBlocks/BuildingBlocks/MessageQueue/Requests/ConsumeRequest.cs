namespace BuildingBlocks.MessageQueue.Requests;

public class ConsumeRequest
{
    private string _exchange;
    public string Exchange
    {
        get => _exchange;
        set => _exchange = value.ToLowerInvariant();
    }
    public string ExchangeType { get; set; }

    public IEnumerable<string> RoutingKeys { get; set; }
}