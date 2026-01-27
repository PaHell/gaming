using backend.Data.Models.Gaming;
using Backend.Data;
using Backend.Data.Repositories.Identity;
using Microsoft.EntityFrameworkCore;

namespace backend.Data.Repositories.Gaming
{
    public interface IUserGameScoreRepository : IUserOwnedEntityRepositoryBase<UserGameScore, Guid>
    {
    }

    public class UserGameScoreRepository(AppDbContext dbContext) : UserOwnedEntityRepositoryBase<UserGameScore, Guid>, IUserGameScoreRepository
    {
        protected override AppDbContext Context => dbContext;

        public override UserGameScore[] GetAllForUser(Guid userId)
        {
            return Context.UserGameScores.Where(ugs => ugs.UserId == userId).ToArray();
        }

        public override Task<UserGameScore[]> GetAllForUserAsync(Guid userId)
        {
            return Context.UserGameScores.Where(ugs => ugs.UserId == userId).ToArrayAsync();
        }

        public override UserGameScore? GetOneForUser(Guid userId, Guid id)
        {
            return Context.UserGameScores.FirstOrDefault(ugs => ugs.Id == id && ugs.UserId == userId);
        }

        public override Task<UserGameScore?> GetOneForUserAsync(Guid userId, Guid id)
        {
            return Context.UserGameScores.FirstOrDefaultAsync(ugs => ugs.Id == id && ugs.UserId == userId);
        }
    }
}
