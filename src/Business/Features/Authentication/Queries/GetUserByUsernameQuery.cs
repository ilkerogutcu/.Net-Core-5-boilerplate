using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Queries
{
    /// <summary>
    /// Get user by username
    /// </summary>
    public class GetUserByUsernameQuery : IRequest<IDataResult<UserResponse>>
    {
        public string Username { get; set; }
    }
}