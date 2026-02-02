using backend.Data.Models.Identity;
using Backend.Data.Enums.Identity;
using Backend.Data.Models.Identity;
using Backend.Data.Repositories.Identity;
using Backend.Extensions.Identity;
using Backend.Services.Identity;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace backend.Controllers.Identity
{
    public class AuthenticationController(IUserRepository userRepository, UserService userService, ILogger<AuthenticationController> logger): Controller
    {
        [HttpPost("/register")]
        public async Task<ActionResult<UserSession>> Register([FromBody] RegisterForm form)
        {
            logger.LogInformation("Registration attempt for email: {Email}", form.Email);
            
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
            
            logger.LogInformation("Registration successful for email: {Email}, SessionId: {SessionId}", form.Email, session.Id);
            return Ok(session);
        }

        [HttpPost("/login")]
        public async Task<ActionResult<UserSession>> Login([FromBody] LoginForm form)
        {
            logger.LogInformation("Login attempt for email: {Email}", form.Email);
            
            // check if user exists
            var user = await userRepository.GetByEmailAsync(form.Email);
            if (user is null)
            {
                logger.LogWarning("Login failed - user not found: {Email}", form.Email);
                return Unauthorized();
            }
            // check password
            var passwordMatch = userService.VerifyPassword(user, form.Password);
            if (!passwordMatch)
            {
                logger.LogWarning("Login failed - invalid password for email: {Email}", form.Email);
                return Unauthorized();
            }
            // sign in
            var session = await userService.SignInAsync(HttpContext, user);
            
            logger.LogInformation("Login successful for email: {Email}, SessionId: {SessionId}", form.Email, session.Id);
            return Ok(session);
        }

        [Authorize]
        [HttpPost("/logout")]
        public async Task<ActionResult<UserSession>> Logout()
        {
            var sessionId = HttpContext.User.GetSessionId();
            logger.LogInformation("Logout attempt for SessionId: {SessionId}", sessionId);
            
            var session = await userService.SignOutAsync(HttpContext);
            if (session is null)
            {
                logger.LogWarning("Logout failed - session not found: {SessionId}", sessionId);
                return Unauthorized(); 
            }
            
            logger.LogInformation("Logout successful for SessionId: {SessionId}", sessionId);
            return Ok(session);
        }
    }
}
