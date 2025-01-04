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

        }
    }
}
