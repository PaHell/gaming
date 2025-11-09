using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Backend.Configuration;
using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Backend.Services.Identity
{
      public class UserService(ApplicationConfiguration configuration, IUserSessionRepository sessionRepository)
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

            private Task<UserSession> CreateSession(User user, string refreshToken)
            {
                  return sessionRepository.CreateAsync(new UserSession
                  {
                        UserId = user.Id,
                        RefreshToken = refreshToken,
                        ExpiresAt = DateTimeOffset.UtcNow.AddSeconds(configuration.Token.RefreshExpirationInSeconds),
                        RevokedAt = null,
                  });
            }

            public async Task<UserSession> SignInAsync(HttpContext httpContext, User user)
            {
                  // create sesion & tokens
                  var refreshToken = GenerateRefreshToken();
                  var session = await CreateSession(user, refreshToken);
                  var accessToken = GenerateAccessToken(user, session.Id);
                  // set cookies
                  httpContext.Response.Cookies.Append(CookieConfiguration.AccessTokenName, accessToken, new CookieOptions
                  {
                        HttpOnly = true,
                        Secure = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = DateTimeOffset.UtcNow.AddSeconds(configuration.Token.AccessExpirationInSeconds),
                        Domain = configuration.Cookie.Domain,
                        Path = configuration.Cookie.Path,
                  });
                  httpContext.Response.Cookies.Append(CookieConfiguration.RefreshTokenName, refreshToken, new CookieOptions
                  {
                        HttpOnly = true,
                        Secure = true,
                        IsEssential = true,
                        SameSite = SameSiteMode.Strict,
                        Expires = session.ExpiresAt,
                        Domain = configuration.Cookie.Domain,
                        Path = configuration.Cookie.Path,
                  });
                  return session;
            }

      }
}