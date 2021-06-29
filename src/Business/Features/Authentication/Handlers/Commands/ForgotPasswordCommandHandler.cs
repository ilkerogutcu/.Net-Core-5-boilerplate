using System;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Features.Authentication.Commands;
using Core.Aspects.Autofac.Logger;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.Mail;
using Core.Utilities.Mail;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Business.Features.Authentication.Handlers.Commands
{
    /// <summary>
    ///     Send forgot password email
    /// </summary>
    public class ForgotPasswordCommandHandler : IRequestHandler<ForgotPasswordCommand, IResult>
    {
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;
        private readonly UserManager<ApplicationUser> _userManager;

        public ForgotPasswordCommandHandler(UserManager<ApplicationUser> userManager, IConfiguration config,
            IMailService mailService)
        {
            _userManager = userManager;
            _config = config;
            _mailService = mailService;
        }

        [LogAspect(typeof(FileLogger))]
        public async Task<IResult> Handle(ForgotPasswordCommand request, CancellationToken cancellationToken)
        {
            try
            {
                var user = await _userManager.FindByNameAsync(request.Username);
                if (user is null) return new ErrorResult(Messages.UserNotFound);
                await SendForgotPasswordEmail(user);
                return new SuccessResult(Messages.SentForgotPasswordEmailSuccessfully);
            }
            catch (Exception e)
            {
                return new ErrorResult(Messages.ForgotPasswordFailed);
            }
        }

        /// <summary>
        ///     Send email
        /// </summary>
        /// <param name="user"></param>
        /// <returns></returns>
        private async Task SendForgotPasswordEmail(ApplicationUser user)
        {
            // Generate token for reset password
            var resetToken = await _userManager.GeneratePasswordResetTokenAsync(user);
            resetToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(resetToken));

            // Generate endpoint url for reset password
            var endPointUrl =
                new Uri(string.Concat($"{_config.GetSection("BaseUrl").Value}", "api/account/reset-password/"));
            var resetTokenUrl = QueryHelpers.AddQueryString(endPointUrl.ToString(), "username", user.UserName);
            resetTokenUrl = QueryHelpers.AddQueryString(resetTokenUrl, "token", resetToken);

            // Edit forgot password email template for reset password link
            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
                @"MailTemplates\SendForgotPasswordEmailTemplate.html");
            using var reader = new StreamReader(emailTemplatePath);
            var mailTemplate = await reader.ReadToEndAsync();
            reader.Close();
            await _mailService.SendEmailAsync(new MailRequest
            {
                ToEmail = user.Email,
                Subject = "You have requested to reset your password",
                Body = mailTemplate.Replace("[resetPasswordLink]", resetTokenUrl)
            });
        }
    }
}