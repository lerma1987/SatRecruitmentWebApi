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
        public UserAuth GetUserById(int id)
        {
            return _unitOfWork.UserAuthRepository.GetUserById(id);
        }
        public ICollection<UserAuth> GetUsers()
        {
            return _unitOfWork.UserAuthRepository.GetUsers();
        }
        public bool IsUniqueUser(string username)
        {
            return _unitOfWork.UserAuthRepository.IsUniqueUser(username);
        }
        public Task<UserLoginResponseDto> Login(UserLoginDto userLoginDto)
        {
            return _unitOfWork.UserAuthRepository.Login(userLoginDto);
        }
        public Task<UserAuth> Register(UserRegisterDto userRegisterDto)
        {
            return _unitOfWork.UserAuthRepository.Register(userRegisterDto);
        }
    }
}
