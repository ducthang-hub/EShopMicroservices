using Basket.API.Persistence.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace Basket.API.Persistence.Repositories;

public interface IUnitOfRepository
{
    public IShoppingCartRepository ShoppingCartRepository { get; set; }
    Task CompleteAsync();
    Task<IDbContextTransaction> OpenTransactionAsync();
    Task CommitAsync();
    Task RollbackAsync();
}