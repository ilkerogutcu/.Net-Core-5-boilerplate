using System;
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
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.Enums;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;

namespace Business.Features.Authentication.Handlers.Commands
{
    /// <summary>
    ///     Sign up for user
    /// </summary>
    [TransactionScopeAspectAsync]
    public class SignUpUserCommandHandler : IRequestHandler<SignUpUserCommand, IDataResult<SignUpResponse>>
    {
        private readonly IAuthenticationMailService _authenticationMailService;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public SignUpUserCommandHandler(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager,
            IMapper mapper, IAuthenticationMailService authenticationMailService)
        {    var mockUserStore = new Mock<IUserStore<ApplicationUser>>();
            _userManager = new UserManager<ApplicationUser>(mockUserStore.Object,
                new Mock<IOptions<IdentityOptions>>().Object,
                new Mock<IPasswordHasher<ApplicationUser>>().Object,
                new IUserValidator<ApplicationUser>[0],
                new IPasswordValidator<ApplicationUser>[0],
                new Mock<ILookupNormalizer>().Object,
                new Mock<IdentityErrorDescriber>().Object,
                new Mock<IServiceProvider>().Object,
                new Mock<ILogger<UserManager<ApplicationUser>>>().Object);
            _roleManager = roleManager;
            _mapper = mapper;
            _authenticationMailService = authenticationMailService;
        }

        /// <summary>
        ///     Create a new user with user role
        /// </summary>
        /// <param name="request"></param>
        /// <param name="cancellationToken"></param>
        /// <returns></returns>
        [ValidationAspect(typeof(SignUpValidator))]
        [LogAspect(typeof(FileLogger))]
        [ExceptionLogAspect(typeof(FileLogger))]
        public async Task<IDataResult<SignUpResponse>> Handle(SignUpUserCommand request,
            CancellationToken cancellationToken)
        {
            var isUserAlreadyExist = await _userManager.FindByNameAsync(request.SignUpRequest.Username);
            if (isUserAlreadyExist is not null)
                return new ErrorDataResult<SignUpResponse>(Messages.UsernameAlreadyExist);

            var isEmailAlreadyExist = await _userManager.FindByEmailAsync(request.SignUpRequest.Username);
            if (isEmailAlreadyExist is not null) return new ErrorDataResult<SignUpResponse>(Messages.EmailAlreadyExist);

            var user = _mapper.Map<ApplicationUser>(request.SignUpRequest);
            var result = await _userManager.CreateAsync(user, request.SignUpRequest.Password);
            if (!result.Succeeded)
            {
                return new ErrorDataResult<SignUpResponse>(Messages.SignUpFailed +
                                                           $":{result.Errors.ToList()[0].Description}");
            }

            if (!await _roleManager.RoleExistsAsync(Roles.User.ToString()))
            {
                await _roleManager.CreateAsync(new IdentityRole(Roles.User.ToString()));
            }

            var rseulee=await _userManager.AddToRoleAsync(user, Roles.User.ToString());
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