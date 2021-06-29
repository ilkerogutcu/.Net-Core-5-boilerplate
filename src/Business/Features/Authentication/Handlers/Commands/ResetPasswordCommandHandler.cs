using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Features.Authentication.Commands;
using Business.Features.Authentication.ValidationRules;
using Core.Aspects.Autofac.Logger;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Business.Features.Authentication.Handlers.Commands
{
    public class ResetPasswordCommandHandler : IRequestHandler<ResetPasswordCommand, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ResetPasswordCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [ValidationAspect(typeof(ResetPasswordValidator))]
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(ResetPasswordCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is null) return new ErrorResult(Messages.UserNotFound);
            request.ResetPasswordToken =
                Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.ResetPasswordToken));
            var result = await _userManager.ResetPasswordAsync(user, request.ResetPasswordToken, request.Password);
            return result.Succeeded
                ? new SuccessResult(Messages.PasswordHasBeenResetSuccessfully)
                : new ErrorResult(Messages.PasswordResetFailed);
        }
    }
}