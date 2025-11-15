using Backend.Configuration;
using Backend.Data.Models;
using Backend.Data.Models.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace Backend.Data
{
      public class AppDbContext(ApplicationConfiguration configuration) : DbContext
      {
            public DbSet<User> Users { get; set; }
            public DbSet<UserSession> UserSessions { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                  base.OnConfiguring(optionsBuilder);
                  optionsBuilder.UseSqlServer(configuration.Database.GetConnectionString());
            }
      }
}