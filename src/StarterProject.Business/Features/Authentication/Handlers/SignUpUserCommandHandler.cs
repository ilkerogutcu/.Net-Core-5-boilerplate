using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.WebUtilities;
using Microsoft.Extensions.Configuration;
using StarterProject.Business.Constants;
using StarterProject.Business.Features.Authentication.Commands;
using StarterProject.Business.Features.Authentication.ValidationRules;
using StarterProject.Core.Aspects.Autofac.Logger;
using StarterProject.Core.Aspects.Autofac.Transaction;
using StarterProject.Core.Aspects.Autofac.Validation;
using StarterProject.Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using StarterProject.Core.Entities.Mail;
using StarterProject.Core.Utilities.Mail;
using StarterProject.Core.Utilities.Results;
using StarterProject.Entities.Concrete;
using StarterProject.Entities.DTOs.Authentication.Responses;
using StarterProject.Entities.Enums;

namespace StarterProject.Business.Features.Authentication.Handlers
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
        public async Task<IDataResult<SignUpResponse>> Handle(SignUpUserCommand command,
            CancellationToken cancellationToken)
        {
            var isUserAlreadyExist = await _userManager.FindByNameAsync(command.Username);
            if (isUserAlreadyExist != null) return new ErrorDataResult<SignUpResponse>(Messages.UsernameAlreadyExist);
            var isEmailAlreadyExist = await _userManager.FindByEmailAsync(command.Username);
            if (isEmailAlreadyExist != null) return new ErrorDataResult<SignUpResponse>(Messages.EmailAlreadyExist);
            var user = new ApplicationUser
            {
                UserName = command.Username,
                Email = command.Email,
                FirstName = command.FirstName,
                LastName = command.LastName
            };
            var result = await _userManager.CreateAsync(user, command.Password);
            if (!result.Succeeded)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.SignUpFailed +
                                                           $":{result.Errors.ToList()[0].Description}");
            }

            if (!await _roleManager.RoleExistsAsync(Roles.User.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }

            await _userManager.AddToRoleAsync(user, Roles.User.ToString());
            var verificationUri = await SendVerificationEmail(user);
            return new SuccessDataResult<SignUpResponse>(new SignUpResponse
            {
                Email = command.Email,
                Username = command.Username
            }, Messages.SignUpSuccessfully + verificationUri);
        }

        private async Task<string> SendVerificationEmail(ApplicationUser user)
        {
            var token = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            token = WebEncoders.Base64UrlEncode(Encoding.UTF8.GetBytes(token));
            var endPointUrl = new Uri(string.Concat($"{_config.GetSection("BaseUrl").Value}", "api/account/confirm-email/"));
            var verificationUrl = QueryHelpers.AddQueryString(endPointUrl.ToString(), "token", token);
            var filePath =Path.Combine(Environment.CurrentDirectory, @"MailTemplates\SendVerificationEmailTemplate.html");
            using (var reader = new StreamReader(filePath))
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
            return QueryHelpers.AddQueryString(verificationUrl, "token", token);
        }
    }
}