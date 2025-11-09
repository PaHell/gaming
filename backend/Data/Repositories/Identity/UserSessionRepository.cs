using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories.Identity
{
      public interface IUserSessionRepository : IUserOwnedEntityRepositoryBase<UserSession, Guid>
      {
      }

      public class UserSessionRepository(AppDbContext dbContext) : UserOwnedEntityRepositoryBase<UserSession, Guid>, IUserSessionRepository
      {
            protected override AppDbContext Context { get => dbContext; }

            public override UserSession[] GetAllForUser(Guid userId)
            {
#pragma warning disable IDE0305 // Simplify collection initialization
                  return Context.UserSessions.Where(s => s.UserId == userId).ToArray();
#pragma warning restore IDE0305 // Simplify collection initialization
            }

            public override Task<UserSession[]> GetAllForUserAsync(Guid userId)
            {
                  return Context.UserSessions.Where(s => s.UserId == userId).ToArrayAsync();
            }

            public override UserSession? GetOneForUser(Guid userId, Guid id)
            {
                  return Context.UserSessions.FirstOrDefault(s => s.UserId == userId && s.Id == id);
            }

            public override Task<UserSession?> GetOneForUserAsync(Guid userId, Guid id)
            {
                  return Context.UserSessions.FirstOrDefaultAsync(s => s.UserId == userId && s.Id == id);
            }
      }
}