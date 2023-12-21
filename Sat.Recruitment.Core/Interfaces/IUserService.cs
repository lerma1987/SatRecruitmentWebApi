using Sat.Recruitment.Core.Entities;
using System.Collections;

namespace Sat.Recruitment.Core.Interfaces
{
    public interface IUserService
    {
        ArrayList InvalidUserTxtLines { get; set; }
        Task<List<IEnumerable<User>>> InsertUserAsync(IEnumerable<User> users);
        IEnumerable<User> GetUsers();
        Task<User> GetUser(int id);
        Task InsertUserAsync(User user);
        Task<bool> UpdateUser(User user);
        Task<bool> DeleteUser(int id);
    }
}
