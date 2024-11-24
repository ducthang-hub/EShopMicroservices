using Basket.API.Domains;
using Basket.API.Persistence.Configurations;
using Microsoft.EntityFrameworkCore;

namespace Basket.API.Persistence.DatabaseContext;

public class BasketDbContext(DbContextOptions<BasketDbContext> options) : DbContext(options)
{
    private const string DefaultSchema = "basket";
    public DbSet<ShoppingCart> ShoppingCarts { get; set; }
    public DbSet<ShoppingCartItem> ShoppingCartItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder
            .ApplyConfiguration(new ShoppingCartConfiguration())
            .ApplyConfiguration(new ShoppingCartItemConfiguration());
    }
}