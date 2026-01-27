namespace backend.Data.Models.Identity
{
    public class TokenResponse
    {
        /// <summary>
        /// Primary access token
        /// </summary>
        public string AccessToken { get; set; } = string.Empty;

        /// <summary>
        /// Token used for refreshing <see cref="AccessToken"/>
        /// </summary>
        public string RefreshToken { get; set; } = string.Empty;
    }
}
