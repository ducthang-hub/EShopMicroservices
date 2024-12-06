using Discount.GRPC.Domains;
using Discount.GRPC.Persistence.DatabaseContext;
using Grpc.Core;
using Mapster;
using Google.Protobuf.WellKnownTypes;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Services;

public class DiscountService(DiscountDbContext dbContext, ILogger<DiscountService> logger) : DiscountProtoService.DiscountProtoServiceBase
{
    public override async Task<CouponModel> CreateDiscount(CreateDiscountRequest request, ServerCallContext context)
    {
        const string functionName = $"{nameof(DiscountService)} - {nameof(CreateDiscount)} =>";
        try
        {
            var newCoupon = new Coupon(request.Coupon);
            newCoupon.PopulateAudit("thang dang", isModified: false);
            await dbContext.Coupons.AddAsync(newCoupon);
            await dbContext.SaveChangesAsync();

            return newCoupon.Adapt<CouponModel>();
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{functionName} Error: {ex.Message}");
            return new CouponModel();
        }
    }

    public override async Task<GetDiscountsResponse> GetDiscounts(Empty request, ServerCallContext context)
    {
        const string functionName = $"{nameof(DiscountService)} - {nameof(GetDiscounts)} =>";
        var response = new GetDiscountsResponse
        {
            Coupons = { }
        };
        try
        {
            var coupons = await dbContext.Coupons.Where(i => !i.IsDeleted).ToListAsync();
            if (!coupons.Any())
            {
                return response;
            }

            response.Coupons.Add(coupons.Adapt<List<CouponModel>>());
        }
        catch (RpcException rpcEx)
        {
            logger.LogError(rpcEx, $"{functionName} Error: {rpcEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{functionName} Error: {ex.Message}");
        }

        return response;
    }
    
    public override async Task<CouponModel?> GetDiscount(GetDiscountRequest request, ServerCallContext context)
    {
        const string functionName = $"{nameof(DiscountService)} - {nameof(GetDiscount)} =>";
        try
        {
            var coupon = await dbContext.Coupons.FirstOrDefaultAsync(i => i.Id.ToString() == request.Id);
            return coupon.Adapt<CouponModel>();
        }
        catch (RpcException rpcEx)
        {
            logger.LogError(rpcEx, $"{functionName} Error: {rpcEx.Message}");
            throw;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{functionName} Error: {ex.Message}");
            return new CouponModel();
        }
    }
}