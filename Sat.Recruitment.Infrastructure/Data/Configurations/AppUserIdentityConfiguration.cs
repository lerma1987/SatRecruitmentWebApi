using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Infrastructure.Data.Configurations
{
    public class AppUserIdentityConfiguration : IEntityTypeConfiguration<AppUser>
    {
        public void Configure(EntityTypeBuilder<AppUser> builder)
        {
            builder.ToTable("AppUser");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("Id");

            builder.Property(e => e.Fullname)
                .HasColumnName("Fullname")
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);
        }
    }
}
