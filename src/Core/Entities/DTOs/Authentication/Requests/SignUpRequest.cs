namespace Core.Entities.DTOs.Authentication.Requests
{
    public class SignUpRequest:IDto
    {
        /// <summary>
        ///     First name
        /// </summary>
        public string FirstName { get; set; }

        /// <summary>
        ///     Last name
        /// </summary>
        public string LastName { get; set; }

        /// <summary>
        ///     Email
        /// </summary>
        public string Email { get; set; }

        /// <summary>
        ///     Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        ///     This variable validates for first password entered
        /// </summary>
        public string ConfirmPassword { get; set; }
    }
}