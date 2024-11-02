using System.Net;
using Basket.API.Domains;
using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using Discount.GRPC;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;

namespace Basket.API.Features.Queries.BasketQueries.GetBasket;

public class GetBasketResponse : ErrorResponse
{
    public ShoppingCart Cart { get; set; }
    public List<CouponModel> Coupons { get; set; }
}

public class GetBasketQuery : IQuery<GetBasketResponse>
{
    public Guid UserId { get; set; }
}

public class GetBasketHandler(IDistributedCache cache, ILogger<GetBasketHandler> logger, DiscountProtoService.DiscountProtoServiceClient discountProtoService) : IQueryHandler<GetBasketQuery, GetBasketResponse>
{
    public async Task<GetBasketResponse> Handle(GetBasketQuery request, CancellationToken cancellationToken)
    {
        const string functionName = $"{nameof(GetBasketHandler)} =>";
        var response = new GetBasketResponse();
        try
        {
            var cartAsString = await cache.GetStringAsync("Carts", cancellationToken);
            if (string.IsNullOrEmpty(cartAsString))
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = $"User {request.UserId} not found";
                return response;
            }

            var cart = JsonConvert.DeserializeObject<ShoppingCart>(cartAsString);
            var coupons = discountProtoService.GetDiscounts(new Empty(), cancellationToken: cancellationToken);

            response.Cart = cart;
            response.Coupons = coupons.Coupons.Adapt<List<CouponModel>>();
            response.Status = HttpStatusCode.OK;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{functionName} Error: {ex.Message}");
            response.Message = ex.Message;
        }

        return response;
    }
}