#region

using Core.Utilities.Results;
using MediatR;

#endregion

namespace Business.Features.Authentication.Commands
{
    public class ForgotPasswordCommand: IRequest<IResult>
    {
        public string Username { get; set; }
    }
}