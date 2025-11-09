using Backend.Data.Models.Base;
using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Base;

namespace Backend.Data.Repositories.Identity
{
      public abstract class ParentedUserOwnedEntityRepositoryBase<ParentKey, Model, ModelKey>
            : RepositoryBase<Model, ModelKey>,
            IParentedUserOwnedEntityRepositoryBase<ParentKey, Model, ModelKey>
          where ParentKey : struct
          where Model : DbEntryBase<Model, ModelKey>
          where ModelKey : struct
      {
            public abstract Model[] GetAllForParent(ParentKey parentId);
            public abstract Task<Model[]> GetAllForParentAsync(ParentKey parentId);
            public abstract Model? GetOneForUser(Guid userId, ModelKey id);
            public abstract Task<Model?> GetOneForUserAsync(Guid userId, ModelKey id);
            public abstract Model[] GetAllForUser(Guid userId);
            public abstract Task<Model[]> GetAllForUserAsync(Guid userId);
      }
}