using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Infrastructure.Data.Configurations
{
    public class UserAuthConfiguration : IEntityTypeConfiguration<UserAuth>
    {
        public void Configure(EntityTypeBuilder<UserAuth> builder)
        {
            builder.ToTable("UsersAuth");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");

            builder.Property(e => e.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Username)
                .HasColumnName("Username")
                .IsRequired()
                .HasMaxLength(15)
                .IsUnicode(false);

            builder.Property(e => e.Password)
                .HasColumnName("Password")
                .IsRequired()
                .HasMaxLength(200)
                .IsUnicode(false);

            builder.Property(e => e.Role)
                .HasColumnName("Role")
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);
        }
    }
}
