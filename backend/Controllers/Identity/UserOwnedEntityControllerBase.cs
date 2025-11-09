using System.Data.SqlTypes;
using System.Security.Claims;
using Backend.Attributes.Identity;
using Backend.Data.Enums.Identity;
using Backend.Data.Models.Base;
using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Base;
using Backend.Data.Repositories.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
namespace Backend.Controllers.Identity
{
      [Authorize]
      [ApiController]
      [Route("api/[controller]")]
      public abstract class UserOwnedEntityControllerBase<TRepository, TModel, TKey> : Controller
            where TRepository : IUserOwnedEntityRepositoryBase<TModel, TKey>
            where TModel : DbEntryBase<TModel, TKey>
            where TKey : struct
      {
            protected abstract TRepository Repository { get; set; }

            [HttpGet("all")]
            [ApplicationPermission(ApplicationRole.Admin)]
            public virtual async Task<IActionResult> GetAll()
            {
                  var items = await this.Repository.GetAllAsync();
                  return Ok(items);
            }

            [HttpGet]
            [ApplicationPermission(ApplicationRole.User)]
            public virtual async Task<IActionResult> GetAllForUser()
            {
                  var userId = User.GetId();
                  var items = await this.Repository.GetAllForUserAsync(userId);
                  return Ok(items);
            }

            [HttpGet("{id}")]
            [ApplicationPermission(ApplicationRole.User)]
            public virtual async Task<IActionResult> GetOne(TKey id)
            {
                  var userId = User.GetId();
                  var item = await this.Repository.GetOneForUserAsync(userId, id);
                  if (item == null)
                  {
                        return NotFound();
                  }
                  return Ok(item);
            }

            [HttpDelete("{id}")]
            [ApplicationPermission(ApplicationRole.Admin)]
            public virtual async Task<IActionResult> Delete(TKey id)
            {
                  var userId = User.GetId();
                  var item = await this.Repository.GetOneForUserAsync(userId, id);
                  if (item == null)
                  {
                        return NotFound();
                  }
                  await this.Repository.DeleteAsync(item);
                  return NoContent();
            }

            [HttpDelete("many")]
            [ApplicationPermission(ApplicationRole.Admin)]
            public virtual async Task<IActionResult> DeleteMany([FromBody] ICollection<TKey> ids)
            {
                  var userId = User.GetId();
                  var items = await this.Repository.GetAllForUserAsync(userId);
                  await this.Repository.DeleteManyAsync([.. items.Where(i => ids.Contains(i.Id))]);
                  return NoContent();
            }
      }
}