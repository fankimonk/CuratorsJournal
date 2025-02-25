using Domain.Enums;
using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace DataAccess.Configurations
{
    public class RolesPermissionsConfiguration : IEntityTypeConfiguration<RolePermission>
    {
        private readonly AuthorizationOptions _authorization;

        public RolesPermissionsConfiguration(AuthorizationOptions authorization)
        {
            _authorization = authorization;
        }

        public void Configure(EntityTypeBuilder<RolePermission> builder)
        {
            builder.HasKey(rp => new { rp.RoleId, rp.PermissionId });

            builder.HasData(ParseRolePermissions());
        }

        private List<RolePermission> ParseRolePermissions()
        {
            return _authorization.RolePermissions
                .SelectMany(rp => rp.Permissions
                    .Select(p => new RolePermission
                    {
                        RoleId = (int)Enum.Parse<Roles>(rp.Role),
                        PermissionId = (int)Enum.Parse<Permissions>(p)
                    }))
                .ToList();
        }
    }
}
