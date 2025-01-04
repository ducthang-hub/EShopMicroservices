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
        }
    }
}
