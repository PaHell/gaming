using System.Security.Claims;
using Backend.Data.Enums.Identity;
using Backend.Data.Models.Identity;

namespace Backend.Extensions.Identity
{
      public static class UserClaimsExtensions
      {
            public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
            {
                  return Guid.Parse(claimsPrincipal.FindFirstValue(UserClaims.UserId)!);
            }

            public static ApplicationRole GetRole(this ClaimsPrincipal claimsPrincipal)
            {
                  return (ApplicationRole)int.Parse(claimsPrincipal.FindFirstValue(UserClaims.Role)!);
            }

            public static Guid GetSessionId(this ClaimsPrincipal claimsPrincipal)
            {
                return Guid.Parse(claimsPrincipal.FindFirstValue(UserClaims.SessionId)!);
            }
    }
}