namespace BuildingBlocks.Protocols.Rpc.RpcClient;

public interface IRpcClient<T>
{
    public Task<T> ProcessUnaryAsync(string queueName, CancellationToken cancellationToken);
}