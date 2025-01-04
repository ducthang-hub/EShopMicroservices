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
        }
    }
}
