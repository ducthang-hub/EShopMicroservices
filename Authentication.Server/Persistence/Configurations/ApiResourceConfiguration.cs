using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Server.Persistence.Configurations
{
    public class ApiResourceConfiguration : IEntityTypeConfiguration<ApiResource>
    {
        public void Configure(EntityTypeBuilder<ApiResource> builder)
        {
            builder.ToTable(nameof(ApiResource), AuthDbContext.DefaultSchema);

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name).IsRequired(); 

            builder.Property(i => i.Secret).IsRequired();

            builder.HasData(
                new ApiResource
                {
                    Id = Guid.Parse("5b9aaeb4-d86c-4b2b-91ff-6df237697b77"),
                    Name = "customer-api-resource",
                    Secret = Guid.Parse("efd456ce-d33b-49f7-bb18-9019338fa79a")
                },
                new ApiResource
                {
                    Id = Guid.Parse("5ed18100-4476-4fd4-ac74-4218e939e3fe"),
                    Name = "shop-owner-api-resource",
                    Secret = Guid.Parse("9c232a28-07ae-4405-96e0-94e57ffb13b0")
                }
            );
        }
    }
}
