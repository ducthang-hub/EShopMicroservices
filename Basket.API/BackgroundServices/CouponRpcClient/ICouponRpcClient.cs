using Discount.GRPC;

namespace Basket.API.BackgroundServices.CouponRpcClient;

public interface ICouponRpcClient
{
    public Task<IEnumerable<CouponModel>> GetCouponsAsync(CancellationToken cancellationToken);

}