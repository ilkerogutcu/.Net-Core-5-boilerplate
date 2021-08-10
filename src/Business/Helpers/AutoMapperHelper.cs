using AutoMapper;
using Core.Entities.DTOs.Authentication.Requests;
using Core.Entities.DTOs.Authentication.Responses;
using Entities.Concrete;
using Entities.DTOs;

namespace Business.Helpers
{
    public class AutoMapperHelper : Profile
    {
        public AutoMapperHelper()
        {
            CreateMap<ApplicationUser, UserResponse>().ReverseMap();
            CreateMap<Product, ProductDto>().ReverseMap();
            CreateMap<ApplicationUser, SignUpRequest>().ReverseMap();

        }
    }
}