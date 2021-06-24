using Core.Utilities.Results;
using Entities.DTOs.Authentication.Responses;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    public class SignInCommand: IRequest<IDataResult<SignInResponse>>
    {
        public string UserName { get; set; }
        public string Password { get; set; }
    }
}