using Discount.GRPC.Domains;
using Discount.GRPC.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Discount.GRPC.Persistence.DatabaseContext;

public class DiscountDbContext(DbContextOptions<DiscountDbContext> options) : DbContext(options)
{
    private const string DefaultSchema = "discount";
    public DbSet<Coupon> Coupons { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfiguration(new CouponConfiguration());
    }
}