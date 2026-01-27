namespace backend.Data.Models.Identity
{
    public class RegisterForm
    {
        /// <inheritdoc cref="User.FirstName"/>
        public string FirstName { get; set; } = string.Empty;

        /// <inheritdoc cref="User.LastName"/>
        public string LastName { get; set; } = string.Empty;

        /// <inheritdoc cref="User.Email"/>
        public string Email { get; set; } = string.Empty;
        
        /// <summary>
        /// Unhashed Password 
        /// </summary>
        public string Password { get; set; } = string.Empty;
    }
}
