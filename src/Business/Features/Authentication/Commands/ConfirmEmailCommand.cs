#region

using Core.Utilities.Results;
using MediatR;

#endregion

namespace Business.Features.Authentication.Commands
{
    public class ConfirmEmailCommand: IRequest<IResult>
    {
        public string UserId { get; set; }
        public string VerificationToken { get; set; }
    }
}