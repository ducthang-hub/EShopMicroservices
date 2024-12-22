using BuildingBlocks.Contracts;
using BuildingBlocks.CQRS;

namespace Catalog.API.Features.Commands.ProductCommands.CreateProduct;

public class CreateProductCommand(string name, double price, string thumbnail, uint piecesAvailable) : ICommand<CreateProductResponse>
{
    public string Name { get; set; } = name;
    public double Price { get; set; } = price;
    public string Thumbnail { get; set; } = thumbnail;
    public uint PiecesAvailable { get; set; } = piecesAvailable;
}

public class CreateProductResponse : ErrorResponse
{
}