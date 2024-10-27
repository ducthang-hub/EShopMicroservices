using BuildingBlocks.Contracts;

namespace Basket.API.Domains;

public class ShoppingCart : AuditData
{
    public Guid Id { get; set; }
    public Guid UserId { get; set; }
    public int ProductQuantity { get; set; }
    public bool IsDeleted { get; set; }
}