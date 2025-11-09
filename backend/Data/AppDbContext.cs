using Backend.Configuration;
using Backend.Data.Models;
using Backend.Data.Models.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
      public class AppDbContext(ApplicationConfiguration configuration) : DbContext
      {
            public DbSet<User> Users { get; set; }
            public DbSet<UserSession> UserSessions { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                  base.OnConfiguring(optionsBuilder);
                  optionsBuilder.UseMySql(
                        configuration.Database.GetConnectionString(),
                        new MySqlServerVersion(new Version(9, 5, 0)),
                        options =>
                        {
                              options.CommandTimeout(60);
                              options.MigrationsHistoryTable("migrations");
                        }
                  );
            }
      }
}