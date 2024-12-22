using Catalog.API.Domains;
using Catalog.API.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Catalog.API.Persistence.DatabaseContext;

public sealed class CatalogDbContext: DbContext
{
    private const string DefaultSchema = "catalog";
    public DbSet<Product> Products { get; init; }

    public CatalogDbContext(DbContextOptions<CatalogDbContext> options) : base(options)
    {
        Products = this.Set<Product>();
    }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfiguration(new ProductConfiguration());
    }
}