using Microsoft.EntityFrameworkCore;
using Sat.Recruitment.Core.Entities;
using System.Reflection;

namespace Sat.Recruitment.Infrastructure.Data
{
    public partial class SatRecruitmentContext : DbContext
    {
        public SatRecruitmentContext() { }
        public SatRecruitmentContext(DbContextOptions<SatRecruitmentContext> options) : base(options) { }

        public virtual DbSet<UserAuth> UsersAuth { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserType> UserTypes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }
    }
}
