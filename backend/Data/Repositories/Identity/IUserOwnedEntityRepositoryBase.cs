using Backend.Data.Models.Base;
using Backend.Data.Repositories.Base;

namespace Backend.Data.Repositories.Identity
{
      public interface IUserOwnedEntityRepositoryBase<Model, Key> : IRepositoryBase<Model, Key>
          where Model : DbEntryBase<Model, Key>
          where Key : struct
      {
            Model? GetOneForUser(Guid userId, Key id);
            Task<Model?> GetOneForUserAsync(Guid userId, Key id);
            Model[] GetAllForUser(Guid userId);
            Task<Model[]> GetAllForUserAsync(Guid userId);
      }
}