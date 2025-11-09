using System.Security.Claims;

namespace Backend.Data.Models.Identity
{
      public static class UserClaims
      {
            public const string SessionId = "session_id";
            public const string UserId = "user_id";
            public const string Role = "role";

            public static IEnumerable<Claim> Create(User user, Guid sessionId)
            {
                  return [
                        new Claim(SessionId, sessionId.ToString()),
                        new Claim(UserId, user.Id.ToString()),
                        new Claim(Role, ((int)user.Role).ToString()),
                  ];
            }
      }
}