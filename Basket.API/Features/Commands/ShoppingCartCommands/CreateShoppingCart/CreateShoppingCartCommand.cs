using BuildingBlocks.Contracts;
using MediatR;

namespace Basket.API.Features.Commands.ShoppingCartCommands.CreateShoppingCart;

public class CreateShoppingCartResponse : ErrorResponse
{
}

public class CreateShoppingCartCommand : IRequest<CreateShoppingCartResponse>
{
}