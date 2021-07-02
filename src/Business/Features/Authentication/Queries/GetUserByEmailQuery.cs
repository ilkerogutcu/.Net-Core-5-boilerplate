using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Queries
{
    /// <summary>
    ///     Get user by email
    /// </summary>
    public class GetUserByEmailQuery : IRequest<IDataResult<UserResponse>>
    {
        public string Email { get; set; }
    }
}