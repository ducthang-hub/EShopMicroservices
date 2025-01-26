namespace Basket.API.Models.DTOs;

public class ProductDto
{
    public Guid Id { get; set; }
    public int Category { get; set; }
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public string Thumbnail { get; set; } = default!;
    public List<string> Images { get; set; } = [];
    public uint PiecesAvailable { get; set; }
}