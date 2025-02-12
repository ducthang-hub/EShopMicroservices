﻿using System.Net;
using Basket.API.Domains;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.CQRS;
using BuildingBlocks.Protocols.Rpc.RpcClient;
using Discount.GRPC;
using Google.Protobuf.WellKnownTypes;

namespace Basket.API.Features.Queries.ShoppingCartQueries.GetShoppingCart;

public class GetShoppingCartHandler : IQueryHandler<GetShoppingCartQuery, GetShoppingCartResponse>
{
    private readonly ILogger<GetShoppingCartHandler> _logger;
    private readonly IBasketRepository _basketRepository;
    private readonly IRpcClient<IEnumerable<Coupon>> _rpcClient;
    private readonly DiscountProtoService.DiscountProtoServiceClient _discountProto;
    
    public GetShoppingCartHandler
    (
        ILogger<GetShoppingCartHandler> logger,
        IBasketRepository basketRepository,
        IRpcClient<IEnumerable<Coupon>> rpcClient,
        DiscountProtoService.DiscountProtoServiceClient discountProto
    )
    {
        _logger = logger;
        _basketRepository = basketRepository;
        _rpcClient = rpcClient;
        _discountProto = discountProto;
    }
    public async Task<GetShoppingCartResponse> Handle(GetShoppingCartQuery request, CancellationToken cancellationToken)
    {
        const string functionName = $"{nameof(GetShoppingCartHandler)} =>";
        var response = new GetShoppingCartResponse();
        
        try
        {
            var cart = await _basketRepository.GetBasketAsync(request.Id, cancellationToken);
            
            // todo: return the old code some day
            // var coupons = await rpcClient.ProcessUnaryAsync("rpc_coupon", cancellationToken);
            var coupons = _discountProto.GetDiscounts(new Empty(), cancellationToken: cancellationToken);
            
            if (cart is null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = $"Cart with user {request.Id} not found";
            }
            else
            {
                response.Data = new
                {
                    Cart = cart,
                    Coupons = coupons
                };
                
                response.Status = HttpStatusCode.OK;
            }
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex, $"{functionName} Error: {ex.Message}");
            response.Message = ex.Message;
        }

        return response;
    }
}