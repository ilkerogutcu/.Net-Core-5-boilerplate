using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using StarterProject.Business.Constants;
using StarterProject.Business.Features.Authentication.Commands;
using StarterProject.Core.Aspects.Autofac.Logger;
using StarterProject.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using StarterProject.Core.Utilities.Results;
using StarterProject.Entities.Concrete;

namespace StarterProject.Business.Features.Authentication.Handlers
{
    public class ConfirmEmailCommandHandler : IRequestHandler<ConfirmEmailCommand, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public ConfirmEmailCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(ConfirmEmailCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.UserId);
            if (user == null) return new ErrorResult(Messages.UserNotFound);
            request.VerificationToken = Encoding.UTF8.GetString(WebEncoders.Base64UrlDecode(request.VerificationToken));
            var result = await _userManager.ConfirmEmailAsync(user, request.VerificationToken);
            return result.Succeeded
                ? (IResult) new SuccessResult(Messages.EmailSuccessfullyConfirmed)
                : new ErrorResult(Messages.ErrorVerifyingMail);
        }
    }
}