using backend.Data.Models.Gaming;
using backend.Data.Repositories.Gaming;
using Backend.Controllers.Identity;

namespace backend.Controllers.Gaming
{
    public class UserGameScoreController(IUserGameScoreRepository userGameScoreRepository) : UserOwnedEntityControllerBase<IUserGameScoreRepository, UserGameScore, Guid>
    {
        protected override IUserGameScoreRepository Repository { get; set; } = userGameScoreRepository;
    }
}
