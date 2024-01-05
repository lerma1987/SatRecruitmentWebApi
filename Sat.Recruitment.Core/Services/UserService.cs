using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Exceptions;
using Sat.Recruitment.Core.Interfaces;
using System.Collections;

namespace Sat.Recruitment.Core.Services
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unitOfWork;
        public ArrayList InvalidUserTxtLines { get; set; }

        public UserService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<List<IEnumerable<UserDetails>>> InsertUserAsync(IEnumerable<UserDetails> users)
        {
            IEnumerable<UserDetails> duplicated, notDuplicated;
            List<IEnumerable<UserDetails>> result;
            try
            {
                result = new List<IEnumerable<UserDetails>>();
                duplicated = await _unitOfWork.UserRepository.FindDuplicatedUsers(users);
                notDuplicated = users.Except(duplicated);
                await _unitOfWork.UserRepository.AddByRange(notDuplicated);
                await _unitOfWork.SaveChangesAsync();

                result.Add(notDuplicated);
                result.Add(duplicated);
            }
            catch (BusinessException)
            {
                throw;
            }
            
            return result;
        }
        public IEnumerable<UserDetails> GetUsers() => _unitOfWork.UserRepository.GetAll();
        public async Task<bool> DeleteUser(int id)
        {
            await _unitOfWork.UserRepository.Delete(id);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }

        public async Task<UserDetails> GetUser(int id)
        {
            return await _unitOfWork.UserRepository.GetById(id);
        }

        public async Task InsertUserAsync(UserDetails user)
        {            
            if (user == null)
                throw new BusinessException("User cannot be null");           

            await _unitOfWork.UserRepository.Add(user);
            await _unitOfWork.SaveChangesAsync();
        }

        public async Task<bool> UpdateUser(UserDetails user)
        {
            var existingUser = await _unitOfWork.UserRepository.GetById(user.Id);
            existingUser.Name = user.Name;
            existingUser.Email = user.Email;
            existingUser.Address = user.Address;
            existingUser.Phone = user.Phone;
            existingUser.Money = user.Money;
            existingUser.UserTypeId = user.UserTypeId;

            _unitOfWork.UserRepository.Update(existingUser);
            await _unitOfWork.SaveChangesAsync();
            return true;
        }        
    }
}
