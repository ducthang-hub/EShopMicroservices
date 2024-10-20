using Catalog.API.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Persistent.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product));

        builder.HasKey(i => i.Id);
        builder.Property(i => i.Name)
            .IsRequired();
        builder.Property(i => i.Price)
            .IsRequired();
        
    }
}