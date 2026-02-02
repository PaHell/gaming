using backend.Data.Models.Identity;
using Backend.Data.Enums.Identity;
using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Identity;
using Backend.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Identity
{
    [ApiController]
    public class AuthenticationController(IUserRepository userRepository, UserService userService): Controller
    {
        [HttpPost("/register")]
        public async Task<ActionResult<UserSessionOut>> Register([FromBody] RegisterForm form)
        {
            var user = new User()
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                Email = form.Email,
                Normalized_Email = form.Email.ToUpperInvariant(),
                PasswordHash = form.Password,
                Role = ApplicationRole.User
            };
            user = userService.SetPassword(user, form.Password);
            var created = await userRepository.CreateAsync(user);
            var session = await userService.SignInAsync(HttpContext, created);
            return Ok(session);
        }

        [HttpPost("/login")]
        public async Task<ActionResult<UserSessionOut>> Login([FromBody] LoginForm form)
        {
            // check if user exists
            var user = await userRepository.GetByEmailAsync(form.Email);
            if (user is null)
            {
                return Unauthorized();
            }
            // check password
            var passwordMatch = userService.VerifyPassword(user, form.Password);
            if (!passwordMatch)
            {
                return Unauthorized();
            }
            // sign in
            var (accessToken, session) = await userService.SignInAsync(HttpContext, user);
            return Ok(new UserSessionOut(session, accessToken));
        }

        [HttpPost("/refresh-token")]
        public async Task<ActionResult<UserSession>> RefreshToken()
        {
            if (!await userService.ValidateTokenAsync(HttpContext, validateLifetime: false))
            {
                return Unauthorized();
            }

            var (accessToken, session) = await userService.RenewSession(HttpContext);
            if (accessToken is null || session is null)
            {
                return Unauthorized();
            }

            return Ok(new UserSessionOut(session, accessToken));
        }

        [Authorize]
        [HttpPost("/logout")]
        public async Task<IActionResult> Logout()
        {
            var session = await userService.SignOutAsync(HttpContext);
            if (session is null)
            {
                return Unauthorized();
            }
            return NoContent();
        }
    }
}
