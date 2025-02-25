using Microsoft.AspNetCore.Authorization;
using Application.Interfaces;
using Microsoft.Extensions.DependencyInjection;

namespace Application.Authorization
{
    public class PermissionAuthorizationHandler : AuthorizationHandler<PermissionRequirement>
    {
        private readonly IServiceScopeFactory _serviceScopeFactory;

        public PermissionAuthorizationHandler(IServiceScopeFactory serviceScopeFactory)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        protected override async Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            var userId = context.User.Claims.FirstOrDefault(c => c.Type == CustomClaims.UserId);

            if (userId == null || !int.TryParse(userId.Value, out int id))
            {
                return;
            }

            using var scope = _serviceScopeFactory.CreateScope();

            var permissionService = scope.ServiceProvider.GetRequiredService<IPermissionsService>();

            var permissions = await permissionService.GetPermissionsAsync(id);

            if (requirement.Permissions.All(permissions.Contains))
            {
                context.Succeed(requirement);
            }
        }
    }
}
