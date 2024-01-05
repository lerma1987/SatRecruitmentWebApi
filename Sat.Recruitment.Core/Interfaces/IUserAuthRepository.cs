
using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Core.Interfaces
{
    public interface IUserAuthRepository
    {
        ICollection<AppUser> GetUsers();
        AppUser GetUserById(string id);
        bool IsUniqueUser(string username);
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
        Task<UserAuthDto> Register(UserRegisterDto userRegisterDto);
    }
}
