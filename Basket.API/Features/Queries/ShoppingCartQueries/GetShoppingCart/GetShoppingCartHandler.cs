using System.Net;
using Basket.API.BackgroundServices.CouponRpcClient;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using Discount.GRPC;
using Google.Protobuf.WellKnownTypes;

namespace Basket.API.Features.Queries.ShoppingCartQueries.GetShoppingCart;

public class GetShoppingCartResponse : ErrorResponse
{
}

public class GetShoppingCartQuery(Guid id) : IQuery<GetShoppingCartResponse>
{
    public Guid Id { get; set; } = id;
}

public class GetShoppingCartHandler
    (
        ILogger<GetShoppingCartHandler> logger,
        DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
        IBasketRepository basketRepository,
        ICouponRpcClient couponRpcClient
    ) 
    : IQueryHandler<GetShoppingCartQuery, GetShoppingCartResponse>
{
    public async Task<GetShoppingCartResponse> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
    {
        const string functionName = $"{nameof(GetShoppingCartHandler)} =>";
        var response = new GetShoppingCartResponse();
        
        try
        {
            var cart = await basketRepository.GetBasketAsync(request.Id, cancellationToken);
            var couponsData = discountProtoServiceClient.GetDiscounts(new Empty(), cancellationToken: cancellationToken);
            var coupons = await couponRpcClient.GetCouponsAsync(cancellationToken);
            // var couponsData = new CouponModel();
            if (cart is null || couponsData is null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = $"Cart with user {request.Id} not found";
            }
            else
            {
                response.Status = HttpStatusCode.OK;
                response.Data = new
                {
                    Cart = cart,
                    Coupons = coupons
                };
            }
        }
        catch (Exception ex)
        {
            logger.LogInformation(ex, $"{functionName} Error: {ex.Message}");
            response.Message = ex.Message;
        }

        return response;
    }
}