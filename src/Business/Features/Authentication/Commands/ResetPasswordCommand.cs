using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    public class ResetPasswordCommand : IRequest<IResult>
    {
        public string Username { get; set; }
        public string ResetPasswordToken { get; set; }
        public string Password { get; set; }
        public string ConfirmPassword { get; set; }
    }
}