using Backend.Data.Enums.Identity;
using Backend.Data.Models.Base;
using Backend.Data.Models.Identity;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace backend.Data.Models.Gaming
{
    public class UserGameScore: DbEntryBase<UserGameScore, Guid>
    {
        public Guid UserId { get; set; }
        public virtual User? User { get; set; }

        public GameType Type { get; set; }
        
        public uint? Score { get; set; }

        public uint? TimeInMilliseconds { get; set; }
        
        public string? Level { get; set; }

        public override void Configure(EntityTypeBuilder<UserGameScore> builder)
        {
            base.Configure(builder);

            builder.HasOne(ugs => ugs.User)
                   .WithMany(u => u.UserGameScores)
                   .HasForeignKey(ugs => ugs.UserId)
                   .OnDelete(Microsoft.EntityFrameworkCore.DeleteBehavior.Cascade);
        }
    }
}
