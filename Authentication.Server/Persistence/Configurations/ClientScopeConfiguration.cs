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
            
            builder.HasData(
                new ClientScope
                {
                    ClientId = Guid.Parse("d120b22d-85e5-4e6f-80fe-ec49e2138d3c"),
                    ApiScopeId = Guid.Parse("75f824ab-b599-4519-8bcc-37bea9ad7cd9")
                },
                new ClientScope
                {
                    ClientId = Guid.Parse("c49ab291-6e10-49bc-a796-848f22a4936f"),
                    ApiScopeId = Guid.Parse("7b3b5167-24a4-41e0-8648-48f7f02dd44b")
                }
            );
        }
    }
}
