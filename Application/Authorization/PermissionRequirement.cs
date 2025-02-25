using Domain.Enums;
using Microsoft.AspNetCore.Authorization;

namespace Application.Authorization
{
    public class PermissionRequirement(Permissions[] permissions) : IAuthorizationRequirement
    {
        public Permissions[] Permissions { get; set; } = permissions;
    }
}
