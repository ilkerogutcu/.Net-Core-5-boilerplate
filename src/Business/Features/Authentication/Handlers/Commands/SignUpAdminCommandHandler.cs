using System;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Abstract;
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
    public class SignUpAdminCommandHandler : IRequestHandler<SignUpAdminCommand, IDataResult<SignUpResponse>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private IAuthenticationMailService _authenticationMailService;

        public SignUpAdminCommandHandler(IMailService mailService, RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, IAuthenticationMailService authenticationMailService)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _authenticationMailService = authenticationMailService;
        }

        [ValidationAspect(typeof(SignUpValidator))]
        [LogAspect(typeof(FileLogger))]
        public async Task<IDataResult<SignUpResponse>> Handle(SignUpAdminCommand request, CancellationToken cancellationToken)
        {
            var isUserAlreadyExist = await _userManager.FindByNameAsync(request.SignUpRequest.Username);
            if (isUserAlreadyExist is not null)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.UsernameAlreadyExist);
            }
            var isEmailAlreadyExist = await _userManager.FindByEmailAsync(request.SignUpRequest.Username);
            if (isEmailAlreadyExist is not null)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.EmailAlreadyExist);
            }
            var user = new ApplicationUser
            {
                UserName = request.SignUpRequest.Username,
                Email = request.SignUpRequest.Email,
                FirstName = request.SignUpRequest.FirstName,
                LastName = request.SignUpRequest.LastName
            };
            var result = await _userManager.CreateAsync(user, request.SignUpRequest.Password);
            if (!result.Succeeded)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.SignUpFailed +
                                                           $":{result.Errors.ToList()[0].Description}");
            }
            if (!await _roleManager.RoleExistsAsync(Roles.Admin.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.Admin.ToString()));
            }
            await _userManager.AddToRoleAsync(user, Roles.Admin.ToString());
            var verificationToken = await _userManager.GenerateEmailConfirmationTokenAsync(user);
            var verificationUri = await _authenticationMailService.SendVerificationEmail(user,verificationToken);
            return new SuccessDataResult<SignUpResponse>(new SignUpResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            }, Messages.SignUpSuccessfully + verificationUri);
        }
    }
}