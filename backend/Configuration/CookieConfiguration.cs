namespace Backend.Configuration
{
      public class CookieConfiguration
      {
            public const string AccessTokenName = "access_token";
            public const string RefreshTokenName = "refresh_token";

            public string Domain { get; set; } = string.Empty;
            public string Path { get; set; } = string.Empty;
      }
}