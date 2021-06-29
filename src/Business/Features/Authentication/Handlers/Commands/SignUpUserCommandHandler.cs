using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Features.Authentication.Commands;
using Business.Features.Authentication.ValidationRules;
using Core.Aspects.Autofac.Logger;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.DTOs.Authentication.Responses;
using Core.Entities.Mail;
using Core.Utilities.Mail;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;

namespace Business.Features.Authentication.Handlers.Commands
{
    [TransactionScopeAspectAsync]
    public class SignUpUserCommandHandler : IRequestHandler<SignUpUserCommand, IDataResult<SignUpResponse>>
    {
        private readonly IConfiguration _config;
        private readonly IMailService _mailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public SignUpUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IMailService mailService, IConfiguration config)
        {
            _userManager = userManager;
            _roleManager = roleManager;
            _mailService = mailService;
            _config = config;
        }

        [ValidationAspect(typeof(SignUpValidator))]
        [LogAspect(typeof(FileLogger))]
        public async Task<IDataResult<SignUpResponse>> Handle(SignUpUserCommand request,
            CancellationToken cancellationToken)
        {
            var isUserAlreadyExist = await _userManager.FindByNameAsync(request.Username);
            if (isUserAlreadyExist is not null)
                return new ErrorDataResult<SignUpResponse>(Messages.UsernameAlreadyExist);

            var isEmailAlreadyExist = await _userManager.FindByEmailAsync(request.Username);
            if (isEmailAlreadyExist is not null) return new ErrorDataResult<SignUpResponse>(Messages.EmailAlreadyExist);

            var user = new ApplicationUser
            {
                UserName = request.Username,
                Email = request.Email,
                FirstName = request.FirstName,
                LastName = request.LastName
            };
            var result = await _userManager.CreateAsync(user, request.Password);
            if (!result.Succeeded)
                return new ErrorDataResult<SignUpResponse>(Messages.SignUpFailed +
                                                           $":{result.Errors.ToList()[0].Description}");

            if (!await _roleManager.RoleExistsAsync(Roles.User.ToString()))
                await _roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));

            await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            var verificationUri = await SendVerificationEmail(user);
            return new SuccessDataResult<SignUpResponse>(new SignUpResponse
            {
                Email = request.Email,
                Username = request.Username
            }, Messages.SignUpSuccessfully + verificationUri);
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user)
        {
            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            verificationToken = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(verificationToken));
            var endPointUrl =
                new Uri(string.Concat($"{_config.GetSection("BaseUrl").Value}", "api/account/confirm-email/"));
            var verificationUrl = QueryHelpers.AddQueryString(endPointUrl.ToString(), "userId", user.Id);
            var emailTemplatePath = Path.Combine(Environment.CurrentDirectory,
                @"MailTemplates\SendVerificationEmailTemplate.html");
            using (var reader = new StreamReader(emailTemplatePath))
            {
                var mailTemplate = await reader.ReadToEndAsync();
                reader.Close();
                await _mailService.SendEmailAsync(new MailRequest
                {
                    ToEmail = user.Email,
                    Subject = "Please verification your email",
                    Body = mailTemplate.Replace("[verificationUrl]", verificationUrl)
                });
            }

            return QueryHelpers.AddQueryString(verificationUrl, "verificationToken", verificationToken);
        }
    }
}