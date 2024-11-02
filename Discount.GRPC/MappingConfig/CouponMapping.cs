using Discount.GRPC.Domains;
using Mapster;

namespace Discount.GRPC.MappingConfig;

public class CouponMapping
{
    public static void RegisterMapping()
    {
        TypeAdapterConfig<Coupon, CouponModel>.NewConfig().Map(dest => dest.Id, src => src.Id.ToString());
    }
}