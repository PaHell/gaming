using Backend.Data.Models.Base;

namespace Backend.Data.Repositories.Identity
{
      public interface IParentedUserOwnedEntityRepositoryBase<ParentKey, Model, ModelKey> : IUserOwnedEntityRepositoryBase<Model, ModelKey>
          where ParentKey : struct
          where Model : DbEntryBase<Model, ModelKey>
          where ModelKey : struct
      {
            Model[] GetAllForParent(ParentKey parentId);
            Task<Model[]> GetAllForParentAsync(ParentKey parentId);
      }
}