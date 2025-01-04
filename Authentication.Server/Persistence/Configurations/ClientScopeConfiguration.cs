using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Server.Persistence.Configurations
{
    public class ClientScopeConfiguration : IEntityTypeConfiguration<ClientScope>
    {
        public void Configure(EntityTypeBuilder<ClientScope> builder)
        {
            builder.ToTable(nameof(ClientScope), AuthDbContext.DefaultSchema);

            builder.HasKey(i => new { i.ClientId, i.ApiScopeId });

            builder.HasOne<Client>(i => i.Client)
                .WithMany(i => i.ClientScopes)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne<ApiScope>(i => i.ApiScope)
                .WithMany(i => i.ClientScopes)
                .HasForeignKey(i => i.ApiScopeId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
