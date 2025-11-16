namespace Backend.Configuration
{
      public class ApplicationConfiguration
      {
            public string AppTitle { get; set; } = string.Empty;
            public string CorsHosts { get; set; } = string.Empty;
            public DatabaseConfiguration Database { get; set; } = new();
            public TokenConfiguration Token { get; set; } = new();
            public CookieConfiguration Cookie { get; set; } = new();
            public string FinnhubApiKey { get; set; } = string.Empty;
            public string AlphaVantageApiKey { get; set; } = string.Empty;
      }
}