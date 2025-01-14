using Authentication.Server.Domains;
using Authentication.Server.Persistence.DatabaseContext;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Authentication.Server.Persistence.Configurations
{
    public class ApiScopeConfiguration : IEntityTypeConfiguration<ApiScope>
    {
        public void Configure(EntityTypeBuilder<ApiScope> builder)
        {
            builder.ToTable(nameof(ApiScope), AuthDbContext.DefaultSchema);

            builder.HasKey(i => i.Id);

            builder.Property(i => i.Name).IsRequired();

            builder.Property(i => i.DisplayName).IsRequired();

            builder.HasData(
                new ApiScope
                {
                    Id = Guid.Parse("75f824ab-b599-4519-8bcc-37bea9ad7cd9"),
                    Name = "customer-scope",
                    DisplayName  = "Student Api Scope"
                },
                new ApiScope
                {
                    Id = Guid.Parse("7b3b5167-24a4-41e0-8648-48f7f02dd44b"),
                    Name = "shop-owner-scope",
                    DisplayName = "Teacher Api Scope"
                }
            );
        }
    }
}
