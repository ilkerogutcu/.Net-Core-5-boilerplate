using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Constants;
using Business.Features.Authentication.Queries;
using Core.Aspects.Autofac.Logger;
using Core.CrossCuttingConcerns.Logging.Serilog.Loggers;
using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Authentication.Handlers.Queries
{
    /// <summary>
    ///     Get user by username
    /// </summary>
    public class GetUserByUsernameQueryHandler : IRequestHandler<GetUserByUsernameQuery, IDataResult<UserResponse>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserByUsernameQueryHandler(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }

        /// <summary>
        ///     Get user by username
        /// </summary>
        [LogAspect(typeof(FileLogger))]
        public async Task<IDataResult<UserResponse>> Handle(GetUserByUsernameQuery request,
            CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByNameAsync(request.Username);
            if (user is null) return new ErrorDataResult<UserResponse>(Messages.UserNotFound);

            var userResponse = _mapper.Map<UserResponse>(user);
            return new SuccessDataResult<UserResponse>(userResponse);
        }
    }
}