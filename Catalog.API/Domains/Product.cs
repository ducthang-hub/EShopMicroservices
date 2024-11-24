using BuildingBlocks.Contracts;

namespace Catalog.API.Domains;

public class Product : AuditData
{
    public Guid Id { get; set; }
    public int Category { get; set; }
    public string Name { get; set; } = default!;
    public double Price { get; set; }
    public string Thumbnail { get; set; } = default!;
    public IEnumerable<string> Images { get; set; }
    public uint PiecesAvailable { get; set; }
}