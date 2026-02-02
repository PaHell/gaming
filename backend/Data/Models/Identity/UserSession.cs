using Backend.Data.Models.Base;

namespace Backend.Data.Models.Identity
{
    public class UserSession : DbEntryBase<UserSession, Guid>
    {
        public required Guid UserId { get; set; }
        public virtual User? User { get; set; }
        public required string RefreshToken { get; set; } = string.Empty;
        public required DateTimeOffset ExpiresAt { get; set; }
        public required DateTimeOffset? RevokedAt { get; set; }
    }
}