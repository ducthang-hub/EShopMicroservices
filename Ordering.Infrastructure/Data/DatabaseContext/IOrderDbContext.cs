using Microsoft.EntityFrameworkCore;
using Ordering.Domain.Models;

namespace Ordering.Infrastructure.Data.DatabaseContext;

public interface IOrderDbContext
{
    public DbSet<Order> Orders { get; }
    public DbSet<OrderItem> OrderItems { get; }
    
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}