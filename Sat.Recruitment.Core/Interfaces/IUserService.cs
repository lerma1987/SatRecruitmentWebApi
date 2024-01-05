using Sat.Recruitment.Core.Entities;
using System.Collections;

namespace Sat.Recruitment.Core.Interfaces
{
    public interface IUserService
    {
        ArrayList InvalidUserTxtLines { get; set; }
        Task<List<IEnumerable<UserDetails>>> InsertUserAsync(IEnumerable<UserDetails> users);
        IEnumerable<UserDetails> GetUsers();
        Task<UserDetails> GetUser(int id);
        Task InsertUserAsync(UserDetails user);
        Task<bool> UpdateUser(UserDetails user);
        Task<bool> DeleteUser(int id);
    }
}
