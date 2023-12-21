using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Core.Interfaces
{
    public interface IUserRepository : IRepository<User>
    {
        Task<IEnumerable<User>> FindByName(string name);
        Task<IEnumerable<User>> FindDuplicatedUsers(IEnumerable<User> usersToFind);
    }
}
