using System.Net;
using Basket.API.DTOs;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;
using Discount.GRPC;
using Google.Protobuf.WellKnownTypes;
using Mapster;
using Microsoft.EntityFrameworkCore;

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
        IUnitOfRepository unitOfRepository,
        ILogger<GetShoppingCartHandler> logger,
        DiscountProtoService.DiscountProtoServiceClient discountProtoServiceClient,
        IBasketRepository basketRepository
    ) 
    : IQueryHandler<GetShoppingCartQuery, GetShoppingCartResponse>
{
    public async Task<GetShoppingCartResponse> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
    {
        const string functionName = $"{nameof(GetShoppingCartHandler)} =>";
        var response = new GetShoppingCartResponse();
        
        try
        {
            var carts = await basketRepository.GetAll();
            var couponsData = discountProtoServiceClient.GetDiscounts(new Empty(), cancellationToken: cancellationToken);
            if (!carts.Any() || couponsData is null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = $"Cart with user {request.Id} not found";
            }
            else
            {
                // var cartDto = carts.Adapt<ShoppingCartDto>();
                response.Status = HttpStatusCode.OK;
                response.Data = new
                {
                    Cart = carts,
                    Coupons = couponsData.Coupons
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