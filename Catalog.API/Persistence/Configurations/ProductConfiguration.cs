using Catalog.API.Domains;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Catalog.API.Persistence.Configurations;

public class ProductConfiguration : IEntityTypeConfiguration<Product>
{
    public void Configure(EntityTypeBuilder<Product> builder)
    {
        builder.ToTable(nameof(Product));

        builder.HasKey(i => i.Id);
        
        builder.Property(i => i.Name)
            .HasMaxLength(250)
            .IsRequired();

        builder.Property(i => i.Thumbnail)
            .HasMaxLength(500)
            .IsRequired();

        builder.Property(i => i.Images)
            .HasColumnType("jsonb")
            .IsRequired(false);
        
        builder.Property(i => i.Price)
            .IsRequired();

        builder.Property(i => i.PiecesAvailable)
            .IsRequired();
    }
}