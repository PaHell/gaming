using backend.Data.Models.Gaming;
using backend.Data.Repositories.Gaming;
using Backend.Controllers.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Gaming
{
    [Authorize]
    [ApiController]
    public class UserGameScoreController(IUserGameScoreRepository userGameScoreRepository) : UserOwnedEntityControllerBase<IUserGameScoreRepository, UserGameScore, Guid>
    {
        protected override IUserGameScoreRepository Repository { get; set; } = userGameScoreRepository;
    }
}
