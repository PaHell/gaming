using System.Security.Claims;
using Backend.Data.Enums.Identity;
using Backend.Data.Models.Identity;

namespace Backend.Extensions.Identity
{
      public static class UserClaimsExtensions
      {
            public static Guid GetId(this ClaimsPrincipal claimsPrincipal)
            {
                  var claim = claimsPrincipal.FindFirstValue(UserClaims.UserId);
                  if (string.IsNullOrEmpty(claim))
                  {
                        throw new UnauthorizedAccessException("User ID claim not found");
                  }
                  return Guid.Parse(claim);
            }

            public static ApplicationRole GetRole(this ClaimsPrincipal claimsPrincipal)
            {
                  var claim = claimsPrincipal.FindFirstValue(UserClaims.Role);
                  if (string.IsNullOrEmpty(claim))
                  {
                        throw new UnauthorizedAccessException("Role claim not found");
                  }
                  return (ApplicationRole)int.Parse(claim);
            }

            public static Guid GetSessionId(this ClaimsPrincipal claimsPrincipal)
            {
                var claim = claimsPrincipal.FindFirstValue(UserClaims.SessionId);
                if (string.IsNullOrEmpty(claim))
                {
                      throw new UnauthorizedAccessException("Session ID claim not found");
                }
                return Guid.Parse(claim);
            }
    }
}