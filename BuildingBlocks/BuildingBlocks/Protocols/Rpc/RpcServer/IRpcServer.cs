namespace BuildingBlocks.Protocols.Rpc.RpcServer;

public interface IRpcServer<T>
{
    public Task ConsumeMessages(string queueName, Func<T> handler, CancellationToken cancellationToken);

}