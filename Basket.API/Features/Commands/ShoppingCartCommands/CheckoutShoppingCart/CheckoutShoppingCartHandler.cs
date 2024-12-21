using System.Net;
using Basket.API.Domains.Events;
using Basket.API.Persistence.Repositories;
using BuildingBlock.Messaging.IntegrationEvents;
using BuildingBlocks.CQRS;
using BuildingBlocks.Extensions.Extensions;
using BuildingBlocks.Helpers;
using BuildingBlocks.MassTransit.Contracts.QueueResponses;
using BuildingBlocks.MassTransit.Contracts.Queues;
using Mapster;
using MassTransit;
using MediatR;

namespace Basket.API.Features.Commands.ShoppingCartCommands.CheckoutShoppingCart;

public class CheckoutShoppingCartHandler(
        IBasketRepository basketRepository,
        ILogger<CheckoutShoppingCartHandler> logger,
        IRequestClient<CheckoutShoppingCartQueueRequest> requestClient,
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

            var @event = payload.Adapt<ShoppingCartCheckoutEvent>(); 
            var cartItemDtos = cart.CartItems.Select(item => item.Adapt<CartItemDto>()).ToList();
            @event.CartItems = cartItemDtos;
            // await publishEndpoint.Publish<ICheckoutShoppingCart>(new {Content = @event}, cancellationToken);
            
            var checkoutCartResponse = await requestClient.GetResponse<CheckoutShoppingCartQueueResponse>(new CheckoutShoppingCartQueueRequest
            {
                Content = @event
            }, cancellationToken);
            if (checkoutCartResponse.Message.InOrderProductIds.Any())
            {
                await publisher.Publish(new CheckoutBasketSuccessfullyEvent
                {
                    CartId = cart.Id,
                    CheckoutProductIds = checkoutCartResponse.Message.InOrderProductIds
                }, cancellationToken);
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