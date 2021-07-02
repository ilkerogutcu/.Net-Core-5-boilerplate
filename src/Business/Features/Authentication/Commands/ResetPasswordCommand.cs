using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    /// <summary>
    ///     Reset password
    /// </summary>
    public class ResetPasswordCommand : IRequest<IResult>
    {
        /// <summary>
        ///     Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Generated token by identity service
        /// </summary>
        public string ResetPasswordToken { get; set; }

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