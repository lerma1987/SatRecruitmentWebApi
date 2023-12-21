using AutoMapper;
using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Infrastructure.Mappings
{
    public class AutomapperProfile : Profile
    {
        public AutomapperProfile()
        {
            CreateMap<User, UserDto>();
            CreateMap<UserDto, User>();

            CreateMap<UserType, UserTypeDto>();
            CreateMap<UserTypeDto, UserType>();

            CreateMap<UserAuth, UserAuthDto>();
            CreateMap<UserAuthDto, UserAuth>();
        }
    }
}
