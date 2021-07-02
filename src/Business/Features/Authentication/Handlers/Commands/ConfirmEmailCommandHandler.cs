using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Features.Authentication.Commands;
using Core.Aspects.Autofac.Logger;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;

namespace Business.Features.Authentication.Handlers.Commands
{
    /// <summary>
    ///     Confirm email
    /// </summary>
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        /// <summary>
        ///     Confirm email
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user is null) return new ErrorResult(Messages.UserNotFound);
            request.VerificationToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.VerificationToken));
            var result = await _userManager.ConfirmEmailAsync(user, request.VerificationToken);
            return result.Succeeded
                ? new SuccessResult(Messages.EmailSuccessfullyConfirmed)
                : new ErrorResult(Messages.ErrorVerifyingMail);
        }
    }
}