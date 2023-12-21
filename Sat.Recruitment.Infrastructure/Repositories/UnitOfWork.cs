using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Infrastructure.Data;

namespace Sat.Recruitment.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SatRecruitmentContext _context;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<UserType> _userTypeRepository;
        private readonly IUserAuthRepository _userAuthRepository;
        public UnitOfWork(SatRecruitmentContext context, IConfiguration config)
        {
            _context = context;
            _config = config;
        }
        public IUserAuthRepository UserAuthRepository => _userAuthRepository ?? new UserAuthRepository(_context, _config);
        public IUserRepository UserRepository => _userRepository ?? new UserRepository(_context);
        public IRepository<UserType> UserTypeRepository => _userTypeRepository ?? new BaseRepository<UserType>(_context);
        public void Dispose()
        {
            if (_context != null)
                _context.Dispose();
        }
        public void SaveChanges()
        {
            _context.SaveChanges();
        }
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
