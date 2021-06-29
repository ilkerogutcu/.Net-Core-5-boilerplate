using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Queries
{
    public class GetUserByUserNameQuery : IRequest<IDataResult<UserResponse>>
    {
        public string Username { get; set; }
    }
}