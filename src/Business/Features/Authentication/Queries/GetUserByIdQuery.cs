using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Queries
{
    /// <summary>
    /// Get user by id
    /// </summary>
    public class GetUserByIdQuery:IRequest<IDataResult<UserResponse>>
    {
        public string Id { get; set; }
    }
}