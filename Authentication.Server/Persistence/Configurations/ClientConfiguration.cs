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

            // builder.HasData(
            //     new Client
            //     {
            //         Id = Guid.NewGuid(),
            //         ClientId = "ewb-student-web"
            //     },
            //     new Client
            //     {
            //         Id = Guid.NewGuid(),
            //         ClientId = "ewb-teacher"
            //     }
            // );
        }
    }
}
