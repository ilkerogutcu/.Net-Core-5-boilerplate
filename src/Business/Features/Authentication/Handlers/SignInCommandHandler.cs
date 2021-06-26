using System;
using System.Globalization;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Business.Constants;
using Business.Features.Authentication.Commands;
using Core.Aspects.Autofac.Logger;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Utilities.Results;
using Entities.Concrete;
using Entities.DTOs.Authentication.Responses;
using MediatR;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;

namespace Business.Features.Authentication.Handlers
{
    public class SignInCommandHandler : IRequestHandler<SignInCommand, IDataResult<SignInResponse>>
    {
        private readonly IConfiguration _configuration;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public SignInCommandHandler(UserManager<ApplicationUser> userManager,
            SignInManager<ApplicationUser> signInManager, IConfiguration configuration)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _configuration = configuration;
        }

        [LogAspect(typeof(FileLogger))]
        public async Task<IDataResult<SignInResponse>> Handle(SignInCommand request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.UserName);
            if (user == null) return new ErrorDataResult<SignInResponse>(Messages.UserNotFound);
            if (!user.EmailConfirmed) return new ErrorDataResult<SignInResponse>(Messages.EmailIsNotConfirmed);

            var result = await _signInManager.PasswordSignInAsync(user.UserName, request.Password,
                false, false);
            if (!result.Succeeded) return new ErrorDataResult<SignInResponse>(Messages.SignInFailed);

            var token = await GenerateJwtToken(user);
            var userRoles = await _userManager.GetRolesAsync(user).ConfigureAwait(false);
            return new SuccessDataResult<SignInResponse>(new SignInResponse
            {
                Email = user.Email,
                Roles = userRoles.ToList(),
                Username = user.UserName,
                IsVerified = user.EmailConfirmed,
                JwtToken = new JwtSecurityTokenHandler().WriteToken(token)
            });
        }

        private async Task<JwtSecurityToken> GenerateJwtToken(ApplicationUser user)
        {
            var userClaims = await _userManager.GetClaimsAsync(user);
            var roles = await _userManager.GetRolesAsync(user);
            var roleClaims = roles.Select(t => new Claim("roles", t)).ToList();
            var ipAddress = Utilities.GetIpAddress();
            var claims = new[]
                {
                    new Claim(JwtRegisteredClaimNames.Sub, user.UserName),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                    new Claim(JwtRegisteredClaimNames.Iss, "https://github.com/ilkerogutcu/.Net-Core-5-boilerplate"),
                    new Claim(JwtRegisteredClaimNames.Email, user.Email),
                    new Claim(JwtRegisteredClaimNames.Iat, DateTime.Now.ToString(CultureInfo.InvariantCulture)),
                    new Claim("ip", ipAddress)
                }
                .Union(userClaims)
                .Union(roleClaims);
            var symmetricSecurityKey =
                new SymmetricSecurityKey(Encoding.ASCII.GetBytes(_configuration["TokenOptions:SecurityKey"]));
            var signingCredentials =
                new SigningCredentials(symmetricSecurityKey, SecurityAlgorithms.HmacSha512Signature);
            return new JwtSecurityToken(
                _configuration["TokenOptions:Issuer"],
                _configuration["TokenOptions:Audience"],
                claims,
                expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_configuration["TokenOptions:DurationInMinutes"])),
                signingCredentials: signingCredentials);
        }
    }
}