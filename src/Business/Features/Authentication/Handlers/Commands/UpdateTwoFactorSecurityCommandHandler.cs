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

namespace Business.Features.Authentication.Handlers.Commands
{
    public class UpdateTwoFactorSecurityCommandHandler : IRequestHandler<UpdateTwoFactorSecurityCommand, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;

        public UpdateTwoFactorSecurityCommandHandler(UserManager<ApplicationUser> userManager)
        {
            _userManager = userManager;
        }

        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(UpdateTwoFactorSecurityCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.userId);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }
            user.TwoFactorEnabled = request.IsEnable;
            var result = await _userManager.UpdateAsync(user);
            return result.Succeeded
                ? new SuccessResult(Messages.UpdatedUserSuccessfully)
                : new ErrorResult(Messages.FailedToUpdateUser);
        }
    }
}