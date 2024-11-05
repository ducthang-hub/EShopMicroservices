using System.Net;
using Basket.API.Domains;
using Basket.API.DTOs;
using Basket.API.Persistence.Repositories;
using BuildingBlocks.Contracts;
using Mapster;
using MediatR;
using Newtonsoft.Json;
using StackExchange.Redis;

namespace Basket.API.Features.Commands.ShoppingCartCommands.CreateShoppingCart;

public class CreateShoppingCartResponse : ErrorResponse
{
}

public class CreateShoppingCartCommand : IRequest<CreateShoppingCartResponse>
{
}

public class CreateShoppingCartHandler
    (
        IUnitOfRepository unitOfRepository,
        ILogger<CreateShoppingCartHandler> logger,
        IConnectionMultiplexer multiplexer
    )
    : IRequestHandler<CreateShoppingCartCommand, CreateShoppingCartResponse>
{
    private readonly IDatabase _redis = multiplexer.GetDatabase();

    public async Task<CreateShoppingCartResponse> Handle(CreateShoppingCartCommand request, CancellationToken cancellationToken)
    {
        const string functionName = $"{nameof(CreateShoppingCartHandler)} =>";
        var response = new CreateShoppingCartResponse();
        
        try
        {
            var newCart = new ShoppingCart
            {
                UserId = Guid.NewGuid().ToString()
            };
            await unitOfRepository.ShoppingCart.Add(newCart);
            await unitOfRepository.CompleteAsync();
            await _redis.HashSetAsync("Basket.ShoppingCart", newCart.UserId, JsonConvert.SerializeObject(newCart));
            var cartDto = newCart.Adapt<ShoppingCartDto>();

            response.Status = HttpStatusCode.OK;
            response.Data = cartDto;
        }
        catch (Exception ex)
        {
            logger.LogError(ex, $"{functionName} Error: {ex.Message}");        
        }

        return response;
    }
}