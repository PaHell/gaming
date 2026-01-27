using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Configuration;
using Backend.Data.Models.General;
using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Identity;
using Backend.Extensions.Identity;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services.Identity
{
      public class UserService(ApplicationConfiguration configuration, IServiceScopeFactory scopeFactory)
      {
            private string GenerateAccessToken(User user, Guid sessionId)
            {
                  // build symmetric key from secret
                  var keyBytes = Encoding.UTF8.GetBytes(configuration.Token.SecretKey);
                  var securityKey = new SymmetricSecurityKey(keyBytes);

                  // choose algorithm
                  var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha512);

                  // create JWT
                  var jwt = new JwtSecurityToken(
                        issuer: configuration.Token.Issuer,
                        audience: configuration.Token.Audience,
                        claims: UserClaims.Create(user, sessionId),
                        expires: DateTime.UtcNow.AddSeconds(configuration.Token.AccessExpirationInSeconds),
                        signingCredentials: credentials
                  );

                  // serialize JWT
                  return new JwtSecurityTokenHandler().WriteToken(jwt);
            }

            private static string GenerateRefreshToken()
            {
                  var bytes = new byte[64]; // 512 bits
                  using var rng = RandomNumberGenerator.Create();
                  rng.GetBytes(bytes);
                  return Convert.ToBase64String(bytes);
            }

        private async Task<UserSession> CreateSession(User user, string refreshToken)
        {
            using var scope = scopeFactory.CreateScope();
            var sessionRepository = scope.ServiceProvider.GetRequiredService<IUserSessionRepository>();
            return await sessionRepository.CreateAsync(new UserSession
            {
                UserId = user.Id,
                RefreshToken = refreshToken,
                ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(configuration.Token.RefreshExpirationInSeconds),
                RevokedAt = null,
            });
        }

        private async Task<UserSession?> RevokeSession(Guid sessionId)
        {
            using var scope = scopeFactory.CreateScope();
            var sessionRepository = scope.ServiceProvider.GetRequiredService<IUserSessionRepository>();
            var session = await sessionRepository.GetByIdAsync(sessionId);
            if (session is null) return null;
            session.RevokedAt = DateTimeOffset.UtcNow;
            var updated = await sessionRepository.UpdateAsync(session);
            var appCache = scope.ServiceProvider.GetRequiredService<AppCache>();
            appCache.RevokedSessionIds.Push(sessionId);
            return updated;
        }

        public bool VerifyPassword(User user, string password)
        {
            var hasher = new PasswordHasher<User>();
            var result = hasher.VerifyHashedPassword(user, user.PasswordHash, password);
            return result == PasswordVerificationResult.Success;
        }

        public User SetPassword(User user, string newPassword)
        {
            var hasher = new PasswordHasher<User>();
            user.PasswordHash = hasher.HashPassword(user, newPassword);
            return user;
        }

        private CookieOptions GetCookieOptions()
        {
            return new()
            {
                HttpOnly = true,
                Secure = true,
                IsEssential = true,
                SameSite = SameSiteMode.Strict,
                Expires = DateTimeOffset.UtcNow.AddSeconds(configuration.Token.AccessExpirationInSeconds),
                Domain = configuration.Cookie.Domain,
                Path = configuration.Cookie.Path,
            };
        }

        public async Task<UserSession> SignInAsync(HttpContext httpContext, User user)
        {
            // create sesion & tokens
            var refreshToken = GenerateRefreshToken();
            var session = await CreateSession(user, refreshToken);
            var accessToken = GenerateAccessToken(user, session.Id);
            // set cookies
            var cookieOptions = GetCookieOptions();
            httpContext.Response.Cookies.Append(CookieConfiguration.AccessTokenName, accessToken, cookieOptions);
            httpContext.Response.Cookies.Append(CookieConfiguration.RefreshTokenName, refreshToken, cookieOptions);
            return session;
        }

        public async Task<UserSession?> SignOutAsync(HttpContext httpContext)
        {
            var sessionId = httpContext.User.GetSessionId();
            return await RevokeSession(sessionId);
        }

    }
}