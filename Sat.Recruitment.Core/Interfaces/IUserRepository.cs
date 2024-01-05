using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Core.Interfaces
{
    public interface IUserRepository : IRepository<UserDetails>
    {
        Task<IEnumerable<UserDetails>> FindByName(string name);
        Task<IEnumerable<UserDetails>> FindDuplicatedUsers(IEnumerable<UserDetails> usersToFind);
    }
}
