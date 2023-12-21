
using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Core.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IRepository<UserType> UserTypeRepository { get; }
        IUserRepository UserRepository { get; }
        IUserAuthRepository UserAuthRepository { get; }
        void SaveChanges();

        Task SaveChangesAsync();
    }
}
