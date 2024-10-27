using Basket.API.Persistence.DatabaseContext;
using Basket.API.Persistence.Repositories.Implements;
using Basket.API.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.Extensions.Caching.Distributed;

namespace Basket.API.Persistence.Repositories;

public class UnitOfRepository : IUnitOfRepository, IDisposable
{
    private readonly BasketDbContext _dbContext;
    private IDbContextTransaction _transaction;

    public IShoppingCartRepository ShoppingCartRepository { get; set; }

    public UnitOfRepository
    ( 
        BasketDbContext dbContext,
        IDistributedCache cache
    )
    {
        _dbContext = dbContext;
        ShoppingCartRepository = new ShoppingCartRepository(_dbContext, cache);
    }
    
    public async Task CompleteAsync()
    {
        await _dbContext.SaveChangesAsync();
    }
        
    public async Task<IDbContextTransaction> OpenTransactionAsync()
    {
        _transaction = await _dbContext.Database.BeginTransactionAsync();
        return _transaction;
    }
    public async Task CommitAsync()
    {
        await _transaction.CommitAsync();
    }

    public async Task RollbackAsync()
    {
        await _transaction.RollbackAsync();
    }

    public void Dispose()
    {
        _dbContext.Dispose();
    }
}