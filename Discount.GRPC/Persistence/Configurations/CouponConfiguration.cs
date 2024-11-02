using Discount.GRPC.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Discount.GRPC.Persistence.Configurations;

public class CouponConfiguration : IEntityTypeConfiguration<Coupon>
{
    public void Configure(EntityTypeBuilder<Coupon> builder)
    {
        builder.ToTable(nameof(Coupon));
        builder.HasKey(i => i.Id);

        builder.Property(i => i.CreateDate)
            .IsRequired();

        builder.Property(i => i.Name)
            .IsRequired();
        
    }
}