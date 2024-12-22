using System.Net;
using Basket.API.Domains.Events;
using Basket.API.Persistence.Repositories;
using BuildingBlock.Messaging.IntegrationEvents;
using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions.Extensions;
using BuildingBlocks.Helpers;
using BuildingBlocks.MassTransit.Contracts.QueueRequests;
using BuildingBlocks.MassTransit.Contracts.QueueResponses;
using Catalog.GRPC;
using Mapster;
using MassTransit;
using MediatR;

namespace Basket.API.Features.Commands.ShoppingCartCommands.CheckoutShoppingCart;

public class CheckoutShoppingCartHandler(
        IBasketRepository basketRepository,
        ILogger<CheckoutShoppingCartHandler> logger,
        IRequestClient<CheckoutShoppingCartQueueRequest> requestClient,
        ProductProtoService.ProductProtoServiceClient productProtoServiceClient,
        IPublisher publisher 
    ) 
    : ICommandHandler<CheckoutShoppingCartCommand, CheckoutShoppingCartResponse>
{
    public async Task<CheckoutShoppingCartResponse> Handle(CheckoutShoppingCartCommand request, CancellationToken cancellationToken)
    {
        var response = new CheckoutShoppingCartResponse();
        try
        {
            var payload = request.Payload; 
            var cart = await basketRepository.GetBasketAsync(payload.CartId, cancellationToken);
            if (cart is null)
            {
                response.Status = HttpStatusCode.NotFound;
                response.Message = $"Shopping cart id {payload.CartId} not found";
                return response;
            }

            var productIds = cart.CartItems.Select(i => i.ProductId.ToString());
            var getProductRequest = new GetProductsRequest();
            foreach (var id in productIds)
            {
                getProductRequest.ProductIds.Add(id);
            }
            var products = productProtoServiceClient.GetProducts(getProductRequest, cancellationToken: cancellationToken);
            
            var @event = payload.Adapt<ShoppingCartCheckoutEvent>(); 
            @event.CartItems = cart.CartItems.Select(item =>
            {
                var product = products.Products.FirstOrDefault(i => i.Id == item.ProductId.ToString());
                var cartItem = item.Adapt<CartItemDto>();
                cartItem.Price = (double)product?.Price!;
                return cartItem;
            }).ToList();
            
            var checkoutCartResponse = await requestClient.GetResponse<CheckoutShoppingCartQueueResponse>(new CheckoutShoppingCartQueueRequest
            {
                Content = @event
            }, cancellationToken);
            if (checkoutCartResponse.Message.InOrderProductIds.Any())
            {
                publisher.Publish(new CheckoutBasketSuccessfullyEvent
                {
                    CartId = cart.Id,
                    CheckoutProductIds = checkoutCartResponse.Message.InOrderProductIds
                }, cancellationToken).FireAndForget();
            }
            
            response.Status = HttpStatusCode.Accepted;
        }
        catch (Exception ex)
        {
            ex.LogError(logger);
        }

        return response;
    }

}