using Basket.API.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Basket.API.Persistence.Configurations;

public class ShoppingCartConfiguration : IEntityTypeConfiguration<ShoppingCart>
{
    public void Configure(EntityTypeBuilder<ShoppingCart> builder)
    {
        builder.ToTable(nameof(ShoppingCart));

        builder.HasKey(i => i.Id);
        builder.Property(i => i.UserId).IsRequired();
    }
}