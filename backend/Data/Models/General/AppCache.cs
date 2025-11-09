using System.Collections.Concurrent;

namespace Backend.Data.Models.General
{
      public class AppCache
      {
            public ConcurrentStack<Guid> RevokedSessionIds { get; } = new ConcurrentStack<Guid>();
      }
}