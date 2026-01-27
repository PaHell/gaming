namespace backend.Data.Models.Identity
{
    public class LoginForm
    {
        /// <inheritdoc cref="User.Email"/>
        public string Email { get; set; } = string.Empty;
        
        /// <inheritdoc cref="RegisterForm.Password"/>
        public string Password { get; set; } = string.Empty;
    }
}
