using Backend.Attributes.Identity;
using Backend.Data.Enums.Identity;
using Backend.Data.Models.Base;
using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Backend.Controllers.Identity
{
      [Authorize]
      [ApiController]
      [Route("api/[controller]")]
      public abstract class ParentedUserOwnedEntityControllerBase<TParentKey, TRepository, TModel, TModelKey>
            : UserOwnedEntityControllerBase<TRepository, TModel, TModelKey>
            where TParentKey : struct
            where TRepository : IParentedUserOwnedEntityRepositoryBase<TParentKey, TModel, TModelKey>
            where TModel : DbEntryBase<TModel, TModelKey>
            where TModelKey : struct
      {
            protected abstract Task<bool> HasAccessToParentAsync(Guid userId, TParentKey parentId);

            [HttpGet("parent/{parentId}")]
            [ApplicationPermission(ApplicationRole.User)]
            public virtual async Task<IActionResult> GetAllForParent(TParentKey parentId)
            {
                  var userId = User.GetId();
                  switch (User.GetRole())
                  {
                        case ApplicationRole.Admin:
                              break;
                        case ApplicationRole.User:
                              if (!await HasAccessToParentAsync(userId, parentId))
                              {
                                    return Forbid();
                              }
                              break;
                  }
                  var item = await Repository.GetAllForParentAsync(parentId);
                  if (item == null)
                  {
                        return NotFound();
                  }
                  return Ok(item);
            }
      }
}