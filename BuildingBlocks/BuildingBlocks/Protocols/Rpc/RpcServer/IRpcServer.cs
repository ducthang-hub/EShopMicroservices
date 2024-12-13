namespace BuildingBlocks.Protocols.Rpc.RpcServer;

public interface IRpcServer
{
    public Task ConsumeMessages(object messageRequest, CancellationToken cancellationToken);

}