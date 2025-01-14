using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Server.Persistence.Configurations
{
    public class ClientSecretConfiguration : IEntityTypeConfiguration<ClientSecret>
    {
        public void Configure(EntityTypeBuilder<ClientSecret> builder)
        {
            builder.ToTable(nameof(ClientSecret), AuthDbContext.DefaultSchema);

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Secret).IsRequired();   

            builder.Property(i => i.ClientId).IsRequired();  

            builder.HasOne(i => i.Client)
                .WithMany(i => i.ClientSecrets)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new ClientSecret
                {
                    ClientId  = Guid.Parse("d120b22d-85e5-4e6f-80fe-ec49e2138d3c"),
                    Secret = "client-web-secret",
                    Id = Guid.Parse("20dd1bb6-e722-41ec-84c4-0d66ac95aa6b")
                },
                new ClientSecret
                {
                    ClientId = Guid.Parse("c49ab291-6e10-49bc-a796-848f22a4936f"),
                    Secret = "client-mobile-secret",
                    Id = Guid.Parse("825c7420-15ba-4d07-87d7-54042b0de9c8")
                }
            );
        }
    }
}
