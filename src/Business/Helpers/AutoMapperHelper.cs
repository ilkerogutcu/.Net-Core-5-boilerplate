#region

using AutoMapper;
using Core.Entities.DTOs.Authentication.Responses;
using Entities.Concrete;

#endregion

namespace Business.Helpers
{
    public class AutoMapperHelper:Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<ApplicationUser, UserResponse>().ReverseMap();
        }
    }
}