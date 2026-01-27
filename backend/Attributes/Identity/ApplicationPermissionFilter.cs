using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Backend.Attributes.Identity
{
    public class ApplicationPermissionFilter : IAsyncAuthorizationFilter
    {
        private readonly IAuthorizationService _authorizationService;

        public ApplicationPermissionFilter(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            // Check if the endpoint has ApplicationPermission attribute
            if (context.ActionDescriptor is ControllerActionDescriptor controllerActionDescriptor)
            {
                var permissionAttribute = controllerActionDescriptor.MethodInfo
                    .GetCustomAttributes(typeof(ApplicationPermissionAttribute), inherit: true)
                    .FirstOrDefault() as ApplicationPermissionAttribute;

                if (permissionAttribute != null)
                {
                    var policyName = $"ApplicationPermission:{permissionAttribute.LowestRequiredRole}";
                    var authResult = await _authorizationService.AuthorizeAsync(
                        context.HttpContext.User,
                        policyName);

                    if (!authResult.Succeeded)
                    {
                        context.Result = new Microsoft.AspNetCore.Mvc.ForbidResult();
                    }
                }
            }
        }
    }
}
