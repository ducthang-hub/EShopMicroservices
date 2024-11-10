using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;
using Ordering.Infrastructure.Data.Configurations;

namespace Ordering.Infrastructure.Data.DatabaseContext;

public class OrderDbContext(DbContextOptions<OrderDbContext> options) : DbContext(options)
{
    private const string DefaultSchema = "order";
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfiguration(new OrderConfiguration())
            .ApplyConfiguration(new OrderItemConfiguration());
    }
}