using Authentication.Server.Domains;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.Persistence.DatabaseContext;

public interface IAuthDbContext
{
    public static string DefaultSchema { get; set; }

    public DbSet<Client> Client { get; }
    public DbSet<User> User { get; }
    public DbSet<ClientGrantType> ClientGrantType { get; }
    public DbSet<ClientScope> ClientScope { get; }
    public DbSet<ClientSecret> ClientSecret { get; }
    public DbSet<ApiResource> ApiResource { get; }
    public DbSet<ApiScope> ApiScope { get; }
    public DbSet<ApiScopeResource> ApiScopeResource { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}