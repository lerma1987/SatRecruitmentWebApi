using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Sat.Recruitment.Core.Entities;
using Sat.Recruitment.Core.Interfaces;
using Sat.Recruitment.Infrastructure.Data;

namespace Sat.Recruitment.Infrastructure.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly SatRecruitmentContext _context;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IConfiguration _config;
        private readonly IUserRepository _userRepository;
        private readonly IRepository<UserType> _userTypeRepository;
        private readonly IUserAuthRepository _userAuthRepository;
        private readonly IMapper _mapper;
        public UnitOfWork(SatRecruitmentContext context, 
                          UserManager<AppUser> userManager, 
                          RoleManager<IdentityRole> roleManager,
                          IConfiguration config, 
                          IMapper mapper)
        {
            _context = context;
            _config = config;
            _mapper = mapper;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        public IUserAuthRepository UserAuthRepository => _userAuthRepository ?? new UserAuthRepository(_context, _userManager, _roleManager, _mapper, _config);
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
