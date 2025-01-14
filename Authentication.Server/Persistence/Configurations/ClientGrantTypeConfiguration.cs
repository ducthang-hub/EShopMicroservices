using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Server.Persistence.Configurations
{
    public class ClientGrantTypeConfiguration : IEntityTypeConfiguration<ClientGrantType>
    {
        public void Configure(EntityTypeBuilder<ClientGrantType> builder)
        {
            builder.ToTable(nameof(ClientGrantType), AuthDbContext.DefaultSchema);

            builder.HasKey(i => i.Id);

            builder.Property(i=>i.ClientId).IsRequired();    

            builder.HasOne(i => i.Client)
                .WithMany(i => i.ClientGrantTypes)
                .HasForeignKey(i => i.ClientId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new ClientGrantType
                {
                    Id = Guid.Parse("eb3cea84-46d3-428f-b5ea-5c42b366c60f"),
                    GrantType = "password",
                    ClientId = Guid.Parse("d120b22d-85e5-4e6f-80fe-ec49e2138d3c")
                }
            );
        }
    }
}
