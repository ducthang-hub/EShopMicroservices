using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Data.Configurations;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.ToTable(nameof(Order));

        builder.HasKey(i => i.Id);

        builder.HasMany(i => i.OrderItems)
            .WithOne()
            .HasForeignKey(i => i.OrderId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(i => i.ShippingAddress)
            .HasColumnType("jsonb");
        
        builder.Property(i => i.Payment)
            .HasColumnType("jsonb");
    }
}