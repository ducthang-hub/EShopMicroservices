using Authentication.Server.Domains;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Authentication.Server.Persistence.DatabaseContext;

public class AuthDbContext(DbContextOptions options) : IdentityDbContext<User>(options)
{
    private const string DefaultSchema = "authen";

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.HasDefaultSchema(DefaultSchema);
    }
}