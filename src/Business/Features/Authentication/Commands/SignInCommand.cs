using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    public class SignInCommand : IRequest<IDataResult<SignInResponse>>
    {
        public string Username { get; set; }
        public string Password { get; set; }
    }
}