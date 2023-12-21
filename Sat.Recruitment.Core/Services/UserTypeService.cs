using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Exceptions;
using Sat.Recruitment.Core.Interfaces;

namespace Sat.Recruitment.Core.Services
{
    public class UserTypeService : IUserTypeService
    {
        private readonly IUnitOfWork _unitOfWork;
        public UserTypeService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public IEnumerable<UserType> GetUserTypes() => _unitOfWork.UserTypeRepository.GetAll();
        public async Task<UserType> GetUserType(int id)
        {
            return await _unitOfWork.UserTypeRepository.GetById(id);
        }
        public async Task InsertUserType(UserType userType)
        {
            if (userType == null)
                throw new BusinessException("UserType cannot be null");

            await _unitOfWork.UserTypeRepository.Add(userType);
            await _unitOfWork.SaveChangesAsync();
        }
        public async Task<bool> UpdateUserType(UserType userType)
        {
            var existingUserType = await _unitOfWork.UserTypeRepository.GetById(userType.Id);
            existingUserType.TypeName = userType.TypeName;

            _unitOfWork.UserTypeRepository.Update(existingUserType);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }
        public async Task<bool> DeleteUserType(int id)
        {
            await _unitOfWork.UserTypeRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

    }
}
