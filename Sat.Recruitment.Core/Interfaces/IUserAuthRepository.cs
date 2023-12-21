
using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Core.Interfaces
{
    public interface IUserAuthRepository
    {
        ICollection<UserAuth> GetUsers();
        UserAuth GetUserById(int id);
        bool IsUniqueUser(string username);
        Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto);
        Task<UserAuth> Register(UserRegisterDto userRegisterDto);
    }
}
