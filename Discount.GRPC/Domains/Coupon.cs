using BuildingBlocks.Contracts;

namespace Discount.GRPC.Domains;

public class Coupon : AuditData
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public double Amount { get; set; }
    public bool IsPercent { get; set; }
    public bool IsDeleted { get; set; }

    public Coupon()
    {
        
    }
    public Coupon(CouponModel model)
    {
        Name = model.Name;
        Description = model.Description;
        Amount = model.Amount;
        IsPercent = model.IsPercent;
    }
}