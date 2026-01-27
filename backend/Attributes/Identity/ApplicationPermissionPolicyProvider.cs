using Backend.Data.Enums.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Options;

namespace Backend.Attributes.Identity
{
    public class ApplicationPermissionPolicyProvider : IAuthorizationPolicyProvider
    {
        private readonly DefaultAuthorizationPolicyProvider _fallbackPolicyProvider;

        public ApplicationPermissionPolicyProvider(IOptions<AuthorizationOptions> options)
        {
            _fallbackPolicyProvider = new DefaultAuthorizationPolicyProvider(options);
        }

        public Task<AuthorizationPolicy> GetDefaultPolicyAsync()
        {
            return _fallbackPolicyProvider.GetDefaultPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetFallbackPolicyAsync()
        {
            return _fallbackPolicyProvider.GetFallbackPolicyAsync();
        }

        public Task<AuthorizationPolicy?> GetPolicyAsync(string policyName)
        {
            // Check if the policy name is for ApplicationPermission
            if (policyName.StartsWith("ApplicationPermission:"))
            {
                var roleString = policyName.Substring("ApplicationPermission:".Length);
                if (Enum.TryParse<ApplicationRole>(roleString, out var role))
                {
                    var policy = new AuthorizationPolicyBuilder()
                        .AddRequirements(new ApplicationPermissionRequirement(role))
                        .Build();
                    return Task.FromResult<AuthorizationPolicy?>(policy);
                }
            }

            // Fall back to default provider
            return _fallbackPolicyProvider.GetPolicyAsync(policyName);
        }
    }
}
