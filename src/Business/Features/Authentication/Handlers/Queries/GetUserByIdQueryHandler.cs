using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Business.Constants;
using Business.Features.Authentication.Queries;
using Core.Entities.DTOs.Authentication.Responses;
using Core.Utilities.Results;
using Entities.Concrete;
using MediatR;
using Microsoft.AspNetCore.Identity;

namespace Business.Features.Authentication.Handlers.Queries
{
    public class GetUserByIdQueryHandler: IRequestHandler<GetUserByIdQuery, IDataResult<UserResponse>>
    {
        private readonly IMapper _mapper;
        private readonly UserManager<ApplicationUser> _userManager;

        public GetUserByIdQueryHandler(IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _mapper = mapper;
            _userManager = userManager;
        }
        
        public async Task<IDataResult<UserResponse>> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
        {
            var user = await _userManager.FindByIdAsync(request.Id);
            if (user is null) return new ErrorDataResult<UserResponse>(Messages.UserNotFound);

            var userResponse = _mapper.Map<UserResponse>(user);
            return new SuccessDataResult<UserResponse>(userResponse);
        }
    }
}