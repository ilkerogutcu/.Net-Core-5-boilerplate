using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Abstract;
using Business.Constants;
using Business.Features.Authentication.Commands;
using Business.Features.Authentication.ValidationRules;
using Core.Aspects.Autofac.Exception;
using Core.Aspects.Autofac.Logger;
using Core.Aspects.Autofac.Transaction;
using Core.Aspects.Autofac.Validation;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Mail;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Authentication.Handlers.Commands
{
    [TransactionScopeAspectAsync]
    public class SignUpAdminCommandHandler : IRequestHandler<SignUpAdminCommand, IDataResult<SignUpResponse>>
    {
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IAuthenticationMailService _authenticationMailService;
        private readonly IMapper _mapper;

        public SignUpAdminCommandHandler(RoleManager<IdentityRole> roleManager, UserManager<ApplicationUser> userManager, 
            IAuthenticationMailService authenticationMailService, IMapper mapper)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _authenticationMailService = authenticationMailService;
            _mapper = mapper;
        }

        [ValidationAspect(typeof(SignUpValidator))]
        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<SignUpResponse>> Handle(SignUpAdminCommand request,
            CancellationToken cancellationToken)
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

            var user = _mapper.Map<ApplicationUser>(request.SignUpRequest);
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
            var verificationUri = await _authenticationMailService.SendVerificationEmail(user, verificationToken);
            return new SuccessDataResult<SignUpResponse>(new SignUpResponse
            {
                Id = user.Id,
                Email = user.Email,
                UserName = user.UserName
            }, Messages.SignUpSuccessfully + verificationUri);
        }
    }
}