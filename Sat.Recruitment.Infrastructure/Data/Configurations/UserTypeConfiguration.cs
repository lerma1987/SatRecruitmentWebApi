using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Infrastructure.Data.Configurations
{
    public class UserTypeConfiguration : IEntityTypeConfiguration<UserType>
    {
        public void Configure(EntityTypeBuilder<UserType> builder)
        {
            builder.ToTable("UserType");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .UseIdentityColumn()
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasColumnName("Id");

            builder.Property(e => e.TypeName)
                .HasColumnName("TypeName")
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.HasMany(d => d.UserDetails)
                .WithOne(p => p.UserType)
                .HasForeignKey(d => d.UserTypeId)
                .OnDelete(DeleteBehavior.ClientSetNull)
                .HasConstraintName("FK_User_UserType");

            builder.HasData(
                new UserType { Id = 1, TypeName = "Normal" },
                new UserType { Id = 2, TypeName = "Premium" },
                new UserType { Id = 3, TypeName = "SuperUser" }
            );
        }
    }
}
