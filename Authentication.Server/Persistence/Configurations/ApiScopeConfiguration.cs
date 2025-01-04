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

            // builder.HasData(
            //     new ApiScope
            //     {
            //         Id = Guid.NewGuid(),
            //         Name = "student-scope",
            //         DisplayName  = "Student Api Scope"
            //     },
            //     new ApiScope
            //     {
            //         Id = Guid.NewGuid(),
            //         Name = "teacher-scope",
            //         DisplayName = "Teacher Api Scope"
            //     }
            // );
        }
    }
}
