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
                  
                  if (!Guid.TryParse(claim, out var userId))
                  {
                        throw new UnauthorizedAccessException("User ID claim has invalid format");
                  }
                  
                  return userId;
            }

            public static ApplicationRole GetRole(this ClaimsPrincipal claimsPrincipal)
            {
                  var claim = claimsPrincipal.FindFirstValue(UserClaims.Role);
                  if (string.IsNullOrEmpty(claim))
                  {
                        throw new UnauthorizedAccessException("Role claim not found");
                  }
                  
                  if (!int.TryParse(claim, out var roleValue))
                  {
                        throw new UnauthorizedAccessException("Role claim has invalid format");
                  }
                  
                  return (ApplicationRole)roleValue;
            }

            public static Guid GetSessionId(this ClaimsPrincipal claimsPrincipal)
            {
                var claim = claimsPrincipal.FindFirstValue(UserClaims.SessionId);
                if (string.IsNullOrEmpty(claim))
                {
                      throw new UnauthorizedAccessException("Session ID claim not found");
                }
                
                if (!Guid.TryParse(claim, out var sessionId))
                {
                      throw new UnauthorizedAccessException("Session ID claim has invalid format");
                }
                
                return sessionId;
            }
    }
}