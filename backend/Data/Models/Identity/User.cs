using backend.Data.Models.Gaming;
using Backend.Data.Enums.Identity;
using Backend.Data.Models.Base;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Backend.Data.Models.Identity
{
      public class User : DbEntryBase<User, Guid>
      {
            public required string FirstName { get; set; } = string.Empty;
            public required string LastName { get; set; } = string.Empty;
            public required string Email { get; set; } = string.Empty;
            public required string Normalized_Email { get; set; } = string.Empty;
            public required string PasswordHash { get; set; } = string.Empty;
            public required ApplicationRole Role { get; set; }

            public virtual ICollection<UserGameScore> UserGameScores { get; set; } = [];

            public override void Configure(EntityTypeBuilder<User> builder)
            {
                  base.Configure(builder);
                  builder.HasIndex(u => u.Normalized_Email).IsUnique();

                  builder.HasMany(u => u.UserGameScores)
                      .WithOne(ugs => ugs.User)
                      .HasForeignKey(ugs => ugs.UserId)
                      .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
            }
      }
}