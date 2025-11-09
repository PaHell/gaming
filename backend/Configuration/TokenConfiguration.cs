namespace Backend.Configuration
{
      public class TokenConfiguration
      {
            public string Issuer { get; set; } = string.Empty;
            public string Audience { get; set; } = string.Empty;
            public string SecretKey { get; set; } = string.Empty;
            public int AccessExpirationInSeconds { get; set; }
            public int RefreshExpirationInSeconds { get; set; }
      }
}