using Backend.Data.Models.Base;
using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Base;

namespace Backend.Data.Repositories.Identity
{
      public abstract class UserOwnedEntityRepositoryBase<Model, Key>
            : RepositoryBase<Model, Key>,
            IUserOwnedEntityRepositoryBase<Model, Key>
          where Model : DbEntryBase<Model, Key>
          where Key : struct
      {
            public abstract Model? GetOneForUser(Guid userId, Key id);
            public abstract Task<Model?> GetOneForUserAsync(Guid userId, Key id);
            public abstract Model[] GetAllForUser(Guid userId);
            public abstract Task<Model[]> GetAllForUserAsync(Guid userId);
      }
}