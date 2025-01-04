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

            // builder.HasData(
            //     new ApiResource
            //     {
            //         Id = Guid.NewGuid(),
            //         Name = "student-api-resource",
            //         Secret = Guid.NewGuid()
            //     },
            //     new ApiResource
            //     {
            //         Id = Guid.NewGuid(),
            //         Name = "teacher-api-resource",
            //         Secret = Guid.NewGuid()
            //     }
            // );
        }
    }
}
