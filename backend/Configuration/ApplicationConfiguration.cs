using backend.Configuration;

namespace Backend.Configuration
{
      public class ApplicationConfiguration
      {
            public string AppTitle { get; set; } = string.Empty;
            public DatabaseConfiguration Database { get; set; } = new();
            public TokenConfiguration Token { get; set; } = new();
            public CookieConfiguration Cookie { get; set; } = new();
            public AlphaVantageConfiguration AlphaVantage { get; set; } = new();
    }
}