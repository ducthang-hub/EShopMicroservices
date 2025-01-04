using Authentication.Server.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.Persistence.DatabaseContext;

public class AuthDbContext(DbContextOptions options) : IdentityDbContext<User>(options), IAuthDbContext
{
    public static string DefaultSchema { get; set; } = "authen";
    
    public DbSet<Client> Client => Set<Client>();
    public DbSet<User> User => Set<User>();
    public DbSet<ClientGrantType> ClientGrantType => Set<ClientGrantType>();
    public DbSet<ClientScope> ClientScope => Set<ClientScope>();
    public DbSet<ClientSecret> ClientSecret => Set<ClientSecret>();
    public DbSet<ApiResource> ApiResource => Set<ApiResource>();
    public DbSet<ApiScope> ApiScope => Set<ApiScope>();
    public DbSet<ApiScopeResource> ApiScopeResource => Set<ApiScopeResource>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(AuthDbContext).Assembly);
    }
}