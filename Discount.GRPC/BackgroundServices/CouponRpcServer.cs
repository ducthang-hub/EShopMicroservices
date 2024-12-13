using BuildingBlocks.Helpers;
using BuildingBlocks.Protocols.Rpc.RpcServer;
using Discount.GRPC.Domains;
using Discount.GRPC.Persistence.DatabaseContext;

namespace Discount.GRPC.BackgroundServices;

public class CouponRpcServer(
    ILogger<CouponRpcServer> logger,
    IRpcServer<IEnumerable<Coupon>> rpcServer,
    IServiceProvider serviceProvider
) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        try
        {
            await rpcServer.ConsumeMessages("rpc_coupon", GetCoupons, cancellationToken);
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }
    }

    private IEnumerable<Coupon> GetCoupons()
    {
        var scope = serviceProvider.CreateScope();
        var dbContext = scope.ServiceProvider.GetService<DiscountDbContext>();
        if (dbContext is null)
        {
            return default!;
        }
            
        var coupons = dbContext.Coupons.AsEnumerable();
        return coupons;
    }
}