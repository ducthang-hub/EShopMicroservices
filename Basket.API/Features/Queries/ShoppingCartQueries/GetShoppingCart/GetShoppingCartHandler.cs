using System.Net;
using Basket.API.Domains;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.CQRS;
using BuildingBlocks.Protocols.Rpc.RpcClient;

namespace Basket.API.Features.Queries.ShoppingCartQueries.GetShoppingCart;

public class GetShoppingCartHandler
    (
        ILogger<GetShoppingCartHandler> logger,
        IBasketRepository basketRepository,
        IRpcClient<IEnumerable<Coupon>> rpcClient
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
            var coupons = await rpcClient.ProcessUnaryAsync("rpc_coupon", cancellationToken);
            
            if (cart is null)
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