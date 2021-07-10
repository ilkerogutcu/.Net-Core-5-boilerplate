using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Queries
{
    public class SignInWithTwoFactorQuery : IRequest<IDataResult<SignInResponse>>
    {
        public string Code { get; set; }
    }
}