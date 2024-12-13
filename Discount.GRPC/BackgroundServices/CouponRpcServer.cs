using BuildingBlocks.Helpers;
using BuildingBlocks.MessageQueue.ConnectionProvider;
using BuildingBlocks.Protocols.Rpc.RpcServer;

namespace Discount.GRPC.BackgroundServices;

public class CouponRpcServer(
    IMessageQueueConnectionProvider connectionProvider,
    ILogger<CouponRpcServer> logger,
    IRpcServer rpcServer
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await rpcServer.ConsumeMessages(null, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }
}