using System;
using System.Threading;
using System.Threading.Tasks;
using Business.Abstract;
using Business.Constants;
using Business.Features.Authentication.Commands;
using Core.Aspects.Autofac.Exception;
using Core.Aspects.Autofac.Logger;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Authentication.Handlers.Commands
{
    /// <summary>
    ///     Send forgot password email
    /// </summary>
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, IResult>
    {
        private readonly IAuthenticationMailService _authenticationMailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager, IAuthenticationMailService authenticationMailService)
        {
            _userManager = userManager;
            _authenticationMailService = authenticationMailService;
        }

        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user is null)
                {
                    return new ErrorResult(Messages.UserNotFound);
                }

                var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
                await _authenticationMailService.SendForgotPasswordEmail(user,resetToken);
                return new SuccessResult(Messages.SentForgotPasswordEmailSuccessfully);
            }
            catch (Exception e)
            {
                return new ErrorResult(Messages.ForgotPasswordFailed);
            }
        }
    }
}