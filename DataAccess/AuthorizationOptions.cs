namespace DataAccess
{
    public class AuthorizationOptions
    {
        public RolesPermissions[] RolePermissions { get; set; } = [];
    }

    public class RolesPermissions
    {
        public string Role { get; set; } = string.Empty;

        public string[] Permissions { get; set; } = [];
    }
}
