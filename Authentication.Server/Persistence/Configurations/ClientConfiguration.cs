using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Server.Persistence.Configurations
{
    public class ClientConfiguration : IEntityTypeConfiguration<Client>
    {
        public void Configure(EntityTypeBuilder<Client> builder)
        {
            builder.ToTable(nameof(Client), AuthDbContext.DefaultSchema);

            builder.HasKey(i => i.Id);

            builder.HasData(
                new Client
                {
                    Id = Guid.Parse("d120b22d-85e5-4e6f-80fe-ec49e2138d3c"),
                    ClientId = "eshop-web"
                },
                new Client
                {
                    Id = Guid.Parse("c49ab291-6e10-49bc-a796-848f22a4936f"),
                    ClientId = "eshop-mobile"
                }
            );
        }
    }
}
