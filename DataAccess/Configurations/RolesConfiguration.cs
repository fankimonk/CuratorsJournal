using Domain.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class RolesConfiguration : IEntityTypeConfiguration<Role>
    {
        public void Configure(EntityTypeBuilder<Role> builder)
        {
            builder.HasKey(r => r.Id);

            var roles = Enum
                .GetValues<Roles>()
                .Select(r => new Role
                {
                    Id = (int)r,
                    Name = r.ToString()
                });

            builder.HasData(roles);
        }
    }
}
