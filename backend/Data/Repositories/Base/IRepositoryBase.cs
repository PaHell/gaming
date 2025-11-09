using Backend.Data.Models.Base;

namespace Backend.Data.Repositories.Base
{
      public interface IRepositoryBase<Model, Key>
          where Model : DbEntryBase<Model, Key>
          where Key : struct
      {
            Model Create(Model entity);
            Task<Model> CreateAsync(Model entity);
            ICollection<Model> CreateMany(ICollection<Model> entities);
            Task<ICollection<Model>> CreateManyAsync(ICollection<Model> entities);
            Model[] GetAll();
            Task<Model[]> GetAllAsync();
            Model? GetById(Key id);
            Task<Model?> GetByIdAsync(Key id);
            Model[] GetByIds(params Key[] ids);
            Task<Model[]> GetByIdsAsync(params Key[] ids);
            Model Update(Model entity);
            Task<Model> UpdateAsync(Model entity);
            ICollection<Model> UpdateMany(ICollection<Model> entities);
            Task<ICollection<Model>> UpdateManyAsync(ICollection<Model> entities);
            void Delete(Model entity);
            Task DeleteAsync(Model entity);
            void DeleteMany(ICollection<Model> entities);
            Task DeleteManyAsync(ICollection<Model> entities);
      }
}