using Backend.Data.Enums.Identity;
using Backend.Extensions.Identity;
using Microsoft.AspNetCore.Authorization;

namespace Backend.Attributes.Identity
{
    public class ApplicationPermissionHandler : AuthorizationHandler<ApplicationPermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, ApplicationPermissionRequirement requirement)
        {
            if (context.User?.Identity?.IsAuthenticated != true)
            {
                return Task.CompletedTask;
            }

            try
            {
                var userRole = context.User.GetRole();
                
                // Check if user's role meets or exceeds the required role
                if (userRole >= requirement.LowestRequiredRole)
                {
                    context.Succeed(requirement);
                }
            }
            catch (UnauthorizedAccessException)
            {
                // Role claim is missing or invalid, fail silently
                // The authorization will fail and return 403
            }

            return Task.CompletedTask;
        }
    }

    public class ApplicationPermissionRequirement : IAuthorizationRequirement
    {
        public ApplicationRole LowestRequiredRole { get; }

        public ApplicationPermissionRequirement(ApplicationRole lowestRequiredRole)
        {
            LowestRequiredRole = lowestRequiredRole;
        }
    }
}
