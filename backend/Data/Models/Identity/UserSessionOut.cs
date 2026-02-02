namespace Backend.Data.Models.Identity
{
    public class UserSessionOut(UserSession i, string accessToken)
    {
        public Guid Id { get; } = i.Id;
        public Guid UserId { get; } = i.UserId;
        public string AccessToken { get; } = accessToken;
        public string RefreshToken { get; } = i.RefreshToken;
        public DateTimeOffset ExpiresAt { get; } = i.ExpiresAt;
        public DateTimeOffset? RevokedAt { get; } = i.RevokedAt;
    }
}