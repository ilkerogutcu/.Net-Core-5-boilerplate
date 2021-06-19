using MediatR;
using StarterProject.Core.Utilities.Results;

namespace StarterProject.Business.Features.Authentication.Commands
{
    public class ConfirmEmailCommand: IRequest<IResult>
    {
        public string UserId { get; set; }
        public string VerificationToken { get; set; }
    }
}