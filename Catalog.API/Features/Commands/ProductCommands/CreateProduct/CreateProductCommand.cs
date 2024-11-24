using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Catalog.API.Features.Commands.ProductCommands.CreateProduct;

public class CreateProductCommand(string name, double price) : ICommand<CreateProductResponse>
{
    public string Name { get; set; } = name;
    public double Price { get; set; } = price;
}

public class CreateProductResponse : ErrorResponse
{
}