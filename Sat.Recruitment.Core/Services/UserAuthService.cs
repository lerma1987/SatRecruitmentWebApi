using Sat.Recruitment.Core.DTOs;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;

namespace Sat.Recruitment.Core.Services
{
    public class UserAuthService : IUserAuthService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserAuthService(IUnitOfWork unitOfWork) 
        { 
            _unitOfWork = unitOfWork;
        }
        public AppUser GetUserById(string id)
        {
            return _unitOfWork.UserAuthRepository.GetUserById(id);
        }
        public ICollection<AppUser> GetUsers()
        {
            return _unitOfWork.UserAuthRepository.GetUsers();
        }
        public bool IsUniqueUser(string username)
        {
            return _unitOfWork.UserAuthRepository.IsUniqueUser(username);
        }
        public async Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            return await _unitOfWork.UserAuthRepository.Login(userLoginDto);
        }
        public async Task<UserAuthDto> Register(UserRegisterDto userRegisterDto)
        {
            return await _unitOfWork.UserAuthRepository.Register(userRegisterDto);
        }
    }
}
