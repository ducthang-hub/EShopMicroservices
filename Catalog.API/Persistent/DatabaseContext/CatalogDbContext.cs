using Catalog.API.Domains;
using Catalog.API.Persistent.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Persistent.DatabaseContext;

public class CatalogDbContext(DbContextOptions<CatalogDbContext> options) : DbContext(options)
{
    private const string DefaultSchema = "catalog";
    public DbSet<Product> Products { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }
}