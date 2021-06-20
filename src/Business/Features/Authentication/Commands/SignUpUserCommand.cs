using Core.Utilities.Results;
using Entities.DTOs.Authentication.Responses;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    public class SignUpUserCommand : IRequest<IDataResult<SignUpResponse>>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string Username { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}