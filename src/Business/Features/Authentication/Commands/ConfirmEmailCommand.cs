using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    /// <summary>
    ///     Confirm email
    /// </summary>
    public class ConfirmEmailCommand : IRequest<IResult>
    {
        /// <summary>
        ///     User id
        /// </summary>
        public string UserId { get; set; }

        /// <summary>
        ///     Generated token by identity service
        /// </summary>
        public string VerificationToken { get; set; }
    }
}