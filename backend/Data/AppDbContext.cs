using backend.Data.Models.Gaming;
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
            public DbSet<UserGameScore> UserGameScores { get; set; }

            protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
            {
                  base.OnConfiguring(optionsBuilder);
                  optionsBuilder
                    .UseMySQL(configuration.Database.GetConnectionString(),
                        options => options.CommandTimeout(0))
                    .UseSnakeCaseNamingConvention();
            }
            protected override void OnModelCreating(ModelBuilder modelBuilder)
            {
                modelBuilder.ApplyConfigurationsFromAssembly(typeof(AppDbContext).Assembly);
            }
      
      }
}