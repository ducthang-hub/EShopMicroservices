namespace Catalog.API.Domains;

public class Product
{
    public Guid Id { get; set; }
    public string Name { get; set; } = default!;
    public double Price { get; set; }
}