using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Basket.API.Features.Commands.ShoppingCartCommands.AddItemsToCart;

public class AddItemsToCartCommand : ICommand<AddItemsToCartResponse>
{
    public AddItemsToCartRequest Payload { get; set; }
}

public class AddItemsToCartResponse : ErrorResponse
{
    
}

public class AddItemsToCartRequest
{
    public Guid ShoppingCartId { get; set; }
    public Guid ProductId { get; set; }
    public uint Quantity { get; set; }
}