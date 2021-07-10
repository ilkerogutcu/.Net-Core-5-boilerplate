using Core.Utilities.Results;
using MediatR;

namespace Business.Features.Authentication.Commands
{
    public class SendEmailConfirmationTokenCommand: IRequest<IResult>
    {
        public string Username { get; set; }
    }
}