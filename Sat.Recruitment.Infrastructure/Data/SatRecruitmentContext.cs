using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Core.Entities;
using System.Reflection;

namespace Sat.Recruitment.Infrastructure.Data
{
    public class SatRecruitmentContext : IdentityDbContext<AppUser>
    {
        public SatRecruitmentContext(DbContextOptions<SatRecruitmentContext> options) : base(options) { }
        public DbSet<UserAuth> UsersAuth { get; set; }
        public DbSet<UserDetails> UserDetails { get; set; }
        public DbSet<AppUser> AppUsers { get; set; }
        public DbSet<UserType> UserTypes { get; set; }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
            base.OnModelCreating(modelBuilder);
        }
    }
}
