using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    public class SignInCommand : IRequest<IDataResult<SignInResponse>>
    {
        /// <summary>
        ///     Username
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        ///     Password
        /// </summary>
        public string Password { get; set; }
    }
}