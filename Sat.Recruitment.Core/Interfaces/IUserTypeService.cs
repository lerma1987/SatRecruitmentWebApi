using Sat.Recruitment.Core.Entities;
using System.Collections;
namespace Sat.Recruitment.Core.Interfaces
{
    public interface IUserTypeService
    {
        IEnumerable<UserType> GetUserTypes();
        Task<UserType> GetUserType(int id);
        Task InsertUserType(UserType user);
        Task<bool> UpdateUserType(UserType user);
        Task<bool> DeleteUserType(int id);
    }
}
