using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    public class ForgotPasswordCommand : IRequest<IResult>
    {
        public string Username { get; set; }
    }
}