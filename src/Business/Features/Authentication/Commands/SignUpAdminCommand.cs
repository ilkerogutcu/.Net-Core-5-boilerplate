using Core.Entities.DTOs.Authentication.Requests;
using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    public class SignUpAdminCommand : IRequest<IDataResult<SignUpResponse>>
    {
        public SignUpRequest SignUpRequest { get; set; }
    }
}