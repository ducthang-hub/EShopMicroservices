using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Basket.API.Features.Commands.ShoppingCartCommands.AddItemsToCart;

public class AddItemsToCartCommand : ICommand<AddItemsToCartResponse>
{
    public AddItemsToCartRequest Payload { get; set; }

    public AddItemsToCartCommand(AddItemsToCartRequest payload)
    {
        Payload = payload;
    }
}

public class AddItemsToCartResponse : ErrorResponse
{
    
}

public class AddItemsToCartRequest
{
    //todo: get UserId from Bearer Token
    public string UserId { get; set; }
    public Guid ShoppingCartId { get; set; }
    public Guid ProductId { get; set; }
    public uint Quantity { get; set; }
}