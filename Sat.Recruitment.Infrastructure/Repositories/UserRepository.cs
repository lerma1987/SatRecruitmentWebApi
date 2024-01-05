using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Infrastructure.Data;
using System.Linq;

namespace Sat.Recruitment.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<UserDetails>, IUserRepository
    {
        public UserRepository(SatRecruitmentContext context) : base(context) { }
        public async Task<IEnumerable<UserDetails>> FindByName(string name)
        {
            return await _entities.Where(x => x.Name.ToLower().Contains(name.ToLower())).ToListAsync();
        }
        public async Task<IEnumerable<UserDetails>> FindDuplicatedUsers(IEnumerable<UserDetails> usersToFind)
        {
            var queryResult = await Task.FromResult((from userItem in usersToFind
                                                     from dbUser in _entities
                                                     where dbUser.Email.ToLower() == userItem.Email.ToLower() ||
                                                           dbUser.Phone.ToLower() == userItem.Phone.ToLower() ||
                                                           dbUser.Address.ToLower() == userItem.Address.ToLower()
                                                     select userItem).AsEnumerable());
            return queryResult;
        }
    }
}