using Backend.Data.Models.Base;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data.Repositories.Base
{
      public abstract class RepositoryBase<Model, Key> : IRepositoryBase<Model, Key>
            where Model : DbEntryBase<Model, Key>
            where Key : struct
      {
            protected abstract AppDbContext Context { get; }

            #region Includes & Sorting

            protected IQueryable<Model> IncludeRelations(IQueryable<Model> query)
            {
                  return query;
            }

            protected IQueryable<Model> SortingOrder(IQueryable<Model> query)
            {
                  return query;
            }

            #endregion

            #region Hooks

            protected Model OnBeforeCreate(Model entity)
            {
                  return entity;
            }

            protected Task<Model> OnBeforeCreateAsync(Model entity)
            {
                  return Task.FromResult(entity);
            }

            protected ICollection<Model> OnBeforeCreateMany(ICollection<Model> entities)
            {
                  return entities;
            }

            protected Task<ICollection<Model>> OnBeforeCreateManyAsync(ICollection<Model> entities)
            {
                  return Task.FromResult(entities);
            }

            protected void OnAfterCreate(Model _)
            {
            }

            protected Task OnAfterCreateAsync(Model _)
            {
                  return Task.CompletedTask;
            }

            protected void OnAfterCreateMany(ICollection<Model> _)
            {
            }

            protected Task OnAfterCreateManyAsync(ICollection<Model> _)
            {
                  return Task.CompletedTask;
            }

            protected Model OnBeforeUpdate(Model entity)
            {
                  return entity;
            }

            protected Task<Model> OnBeforeUpdateAsync(Model entity)
            {
                  return Task.FromResult(entity);
            }

            protected ICollection<Model> OnBeforeUpdateMany(ICollection<Model> entities)
            {
                  return entities;
            }

            protected Task<ICollection<Model>> OnBeforeUpdateManyAsync(ICollection<Model> entities)
            {
                  return Task.FromResult(entities);
            }

            protected void OnAfterUpdate(Model _)
            {
            }

            protected Task OnAfterUpdateAsync(Model _)
            {
                  return Task.CompletedTask;
            }

            protected void OnAfterUpdateMany(ICollection<Model> _)
            {
            }

            protected Task OnAfterUpdateManyAsync(ICollection<Model> _)
            {
                  return Task.CompletedTask;
            }

            protected Model OnBeforeCreateOrUpdate(Model entity)
            {
                  return entity;
            }

            protected Task<Model> OnBeforeCreateOrUpdateAsync(Model entity)
            {
                  return Task.FromResult(entity);
            }

            protected ICollection<Model> OnBeforeCreateOrUpdateMany(ICollection<Model> entities)
            {
                  return entities;
            }

            protected Task<ICollection<Model>> OnBeforeCreateOrUpdateManyAsync(ICollection<Model> entities)
            {
                  return Task.FromResult(entities);
            }

            protected void OnAfterCreateOrUpdate(Model _)
            {
            }

            protected Task OnAfterCreateOrUpdateAsync(Model _)
            {
                  return Task.CompletedTask;
            }

            protected void OnAfterCreateOrUpdateMany(ICollection<Model> _)
            {
            }

            protected Task OnAfterCreateOrUpdateManyAsync(ICollection<Model> _)
            {
                  return Task.CompletedTask;
            }

            protected void OnBeforeDelete(Model _)
            {
            }

            protected Task OnBeforeDeleteAsync(Model _)
            {
                  return Task.CompletedTask;
            }

            protected void OnBeforeDeleteMany(ICollection<Model> _)
            {
            }

            protected Task OnBeforeDeleteManyAsync(ICollection<Model> _)
            {
                  return Task.CompletedTask;
            }

            #endregion

            #region Create

            public Model Create(Model entity)
            {
                  entity = OnBeforeCreateOrUpdate(OnBeforeCreate(entity));
                  Context.Set<Model>().Add(entity);
                  Context.SaveChanges();
                  OnAfterCreate(entity);
                  OnAfterCreateOrUpdate(entity);
                  return GetById(entity.Id!)!;
            }

            public async Task<Model> CreateAsync(Model entity)
            {
                  entity = await OnBeforeCreateOrUpdateAsync(await OnBeforeCreateAsync(entity));
                  await Context.Set<Model>().AddAsync(entity);
                  await Context.SaveChangesAsync();
                  await OnAfterCreateAsync(entity);
                  await OnAfterCreateOrUpdateAsync(entity);
                  return (await GetByIdAsync(entity.Id!)!)!;
            }

            public ICollection<Model> CreateMany(ICollection<Model> entities)
            {
                  entities = OnBeforeCreateOrUpdateMany(OnBeforeCreateMany(entities));
                  Context.Set<Model>().AddRange(entities);
                  Context.SaveChanges();
                  OnAfterCreateMany(entities);
                  OnAfterCreateOrUpdateMany(entities);
                  return SortingOrder(
                              IncludeRelations(Context
                                    .Set<Model>()
                                    .Where(e => entities.Select(ent => ent.Id).Contains(e.Id))
                        )).ToArray();
            }

            public async Task<ICollection<Model>> CreateManyAsync(ICollection<Model> entities)
            {
                  entities = await OnBeforeCreateOrUpdateManyAsync(await OnBeforeCreateManyAsync(entities));
                  await Context.Set<Model>().AddRangeAsync(entities);
                  await Context.SaveChangesAsync();
                  await OnAfterCreateManyAsync(entities);
                  await OnAfterCreateOrUpdateManyAsync(entities);
                  return await SortingOrder(
                              IncludeRelations(Context
                                    .Set<Model>()
                                    .Where(e => entities.Select(ent => ent.Id).Contains(e.Id))
                        )).ToArrayAsync();
            }

            #endregion

            #region Read

            public Model[] GetAll()
            {
#pragma warning disable IDE0305 // Simplify collection initialization
                  return SortingOrder(
                              IncludeRelations(Context.Set<Model>())
                        ).ToArray();
#pragma warning restore IDE0305 // Simplify collection initialization
            }

            public Task<Model[]> GetAllAsync()
            {
                  return SortingOrder(
                              IncludeRelations(Context.Set<Model>())
                        ).ToArrayAsync();
            }

            public Model? GetById(Key id)
            {
                  return IncludeRelations(Context.Set<Model>())
                        .FirstOrDefault(e => e.Id!.Equals(id));
            }

            public Task<Model?> GetByIdAsync(Key id)
            {
                  return IncludeRelations(Context.Set<Model>())
                        .FirstOrDefaultAsync(e => e.Id!.Equals(id));
            }

            public Model[] GetByIds(params Key[] ids)
            {
#pragma warning disable IDE0305 // Simplify collection initialization
                  return SortingOrder(
                              IncludeRelations(Context
                                    .Set<Model>()
                                    .Where(e => ids.Contains(e.Id!))
                        )).ToArray();
#pragma warning restore IDE0305 // Simplify collection initialization
            }

            public Task<Model[]> GetByIdsAsync(params Key[] ids)
            {
                  return SortingOrder(
                              IncludeRelations(Context
                                    .Set<Model>()
                                    .Where(e => ids.Contains(e.Id!))
                        )).ToArrayAsync();
            }

            #endregion

            #region Update

            public Model Update(Model entity)
            {
                  entity = OnBeforeCreateOrUpdate(OnBeforeUpdate(entity));
                  Context.Set<Model>().Update(entity);
                  Context.SaveChanges();
                  OnAfterUpdate(entity);
                  OnAfterCreateOrUpdate(entity);
                  return GetById(entity.Id!)!;
            }

            public async Task<Model> UpdateAsync(Model entity)
            {
                  entity = await OnBeforeCreateOrUpdateAsync(await OnBeforeUpdateAsync(entity));
                  Context.Set<Model>().Update(entity);
                  await Context.SaveChangesAsync();
                  await OnAfterUpdateAsync(entity);
                  await OnAfterCreateOrUpdateAsync(entity);
                  return (await GetByIdAsync(entity.Id!)!)!;
            }

            public ICollection<Model> UpdateMany(ICollection<Model> entities)
            {
                  entities = OnBeforeCreateOrUpdateMany(OnBeforeUpdateMany(entities));
                  Context.Set<Model>().UpdateRange(entities);
                  Context.SaveChanges();
                  OnAfterUpdateMany(entities);
                  OnAfterCreateOrUpdateMany(entities);
                  return GetByIds([.. entities.Select(e => e.Id!)]);
            }

            public async Task<ICollection<Model>> UpdateManyAsync(ICollection<Model> entities)
            {
                  entities = await OnBeforeCreateOrUpdateManyAsync(await OnBeforeUpdateManyAsync(entities));
                  Context.Set<Model>().UpdateRange(entities);
                  await Context.SaveChangesAsync();
                  await OnAfterUpdateManyAsync(entities);
                  await OnAfterCreateOrUpdateManyAsync(entities);
                  return await GetByIdsAsync([.. entities.Select(e => e.Id!)]);
            }

            #endregion

            #region Delete

            public void Delete(Model entity)
            {
                  OnBeforeDelete(entity);
                  Context.Set<Model>().Remove(entity);
                  Context.SaveChanges();
            }

            public async Task DeleteAsync(Model entity)
            {
                  await OnBeforeDeleteAsync(entity);
                  Context.Set<Model>().Remove(entity);
                  await Context.SaveChangesAsync();
            }

            public void DeleteMany(ICollection<Model> entities)
            {
                  OnBeforeDeleteMany(entities);
                  Context.Set<Model>().RemoveRange(entities);
                  Context.SaveChanges();
            }

            public async Task DeleteManyAsync(ICollection<Model> entities)
            {
                  await OnBeforeDeleteManyAsync(entities);
                  Context.Set<Model>().RemoveRange(entities);
                  await Context.SaveChangesAsync();
            }

            #endregion
      }
}