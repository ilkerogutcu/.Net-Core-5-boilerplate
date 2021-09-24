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
    public class SendEmailConfirmationTokenCommandHandler: IRequestHandler<SendEmailConfirmationTokenCommand, IResult>
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationMailService _authenticationMailService;

        public SendEmailConfirmationTokenCommandHandler(UserManager<ApplicationUser> userManager, IAuthenticationMailService authenticationMailService)
        {
            _userManager = userManager;
            _authenticationMailService = authenticationMailService;
        }
        
        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(SendEmailConfirmationTokenCommand request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is null)
            {
                return new ErrorResult(Messages.UserNotFound);
            }

            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            await _authenticationMailService.SendVerificationEmail(user, verificationToken);
            return new SuccessResult(Messages.SentConfirmationEmailSuccessfully);
        }
    }
}