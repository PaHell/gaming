namespace Backend.Configuration
{
      public class DatabaseConfiguration
      {
            public string Host { get; set; } = string.Empty;
            public uint Port { get; set; }
            public string Name { get; set; } = string.Empty;
            public string User { get; set; } = string.Empty;
            public string Password { get; set; } = string.Empty;

            public string GetConnectionString()
            {
                  return $"Host={Host};Port={Port};Database={Name};Username={User};Password={Password}";
            }
      }
}