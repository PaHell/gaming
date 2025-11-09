using System.Security.Claims;
using Backend.Data.Enums.Identity;

namespace Backend.Data.Models.Identity
{
      public static class UserClaimsExtensions
      {
            public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
            {
                  return Guid.Parse(claimsPrincipal.FindFirstValue(UserClaims.Id)!);
            }

            public static ApplicationRole GetRole(this ClaimsPrincipal claimsPrincipal)
            {
                  return (ApplicationRole)int.Parse(claimsPrincipal.FindFirstValue(UserClaims.Role)!);
            }
      }
}