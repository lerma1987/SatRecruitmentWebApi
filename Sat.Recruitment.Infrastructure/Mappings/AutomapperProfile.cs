using AutoMapper;
using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<UserDetails, UserDto>();
            CreateMap<UserDto, UserDetails>();

            CreateMap<UserType, UserTypeDto>();
            CreateMap<UserTypeDto, UserType>();

            CreateMap<UserAuth, UserAuthDto>();
            CreateMap<UserAuthDto, UserAuth>();

            CreateMap<AppUser, UserAuthDto>();
            CreateMap<UserAuthDto, AppUser>();
        }
    }
}
