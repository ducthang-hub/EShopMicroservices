using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Server.Persistence.Configurations
{
    public class ApiScopeResourceConfiguration : IEntityTypeConfiguration<ApiScopeResource>
    {
        public void Configure(EntityTypeBuilder<ApiScopeResource> builder)
        {
            builder.ToTable(nameof(ApiScopeResource), AuthDbContext.DefaultSchema);

            builder.HasKey(i => new {i.ApiScopeId, i.ApiResourceId});

            builder.HasOne(i => i.ApiScope)
                .WithMany(i => i.ApiScopeResources)
                .HasForeignKey(i => i.ApiScopeId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(i => i.ApiResource)
                .WithMany(i => i.ApiScopeResources)
                .HasForeignKey(i => i.ApiResourceId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.HasData(
                new ApiScopeResource
                {
                    ApiResourceId = Guid.Parse("5b9aaeb4-d86c-4b2b-91ff-6df237697b77"),
                    ApiScopeId = Guid.Parse("75f824ab-b599-4519-8bcc-37bea9ad7cd9"),
                },
                new ApiScopeResource
                {
                    ApiResourceId = Guid.Parse("5ed18100-4476-4fd4-ac74-4218e939e3fe"),
                    ApiScopeId = Guid.Parse("7b3b5167-24a4-41e0-8648-48f7f02dd44b"),
                }
            );
        }
    }
}
