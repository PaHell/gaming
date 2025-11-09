using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Base;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories.Identity
{
      public interface IUserRepository : IRepositoryBase<User, Guid>
      {
            User? GetByEmail(string email);
            Task<User?> GetByEmailAsync(string email);
      }

      public class UserRepository(AppDbContext dbContext) : RepositoryBase<User, Guid>, IUserRepository
      {
            protected override AppDbContext Context { get => dbContext; }

            public User? GetByEmail(string email)
            {
                  var upperEmail = email.ToUpperInvariant();
                  return Context.Users.FirstOrDefault(u => u.Normalized_Email == upperEmail);
            }

            public Task<User?> GetByEmailAsync(string email)
            {
                  var upperEmail = email.ToUpperInvariant();
                  return Context.Users.FirstOrDefaultAsync(u => u.Normalized_Email == upperEmail);
            }
      }
}