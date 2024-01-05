using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore.Migrations;
using Sat.Recruitment.Core.Entities;
using System.Reflection.Emit;

namespace Sat.Recruitment.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<UserDetails>
    {
        public void Configure(EntityTypeBuilder<UserDetails> builder)
        {
            builder.ToTable("UserDetails");

            builder.HasKey(e => e.UserDetailId);
            builder.HasAlternateKey(c => new { c.Email, c.Phone, c.Address }).HasName("IX_EmailPhoneAddress");

            builder.Property(e => e.UserDetailId)
                .UseIdentityColumn()
                .HasColumnType("int")
                .ValueGeneratedOnAdd()
                .HasColumnName("UserDetailId");

            builder.Property(e => e.Name)
                .HasColumnName("Name")
                .IsRequired()
                .HasMaxLength(50)
                .IsUnicode(false);

            builder.Property(e => e.Email)
                .HasColumnName("Email")
                .IsRequired()
                .HasMaxLength(30)
                .IsUnicode(false);

            builder.Property(e => e.Address)
                .HasColumnName("Address")
                .IsRequired()
                .HasMaxLength(100)
                .IsUnicode(false);

            builder.Property(e => e.Phone)
                .HasColumnName("Phone")
                .IsRequired()
                .HasMaxLength(13)
                .IsUnicode(false);

            builder.Property(e => e.UserTypeId)
               .HasColumnName("UserTypeId")
               .IsRequired();

            builder.Property(e => e.Money)
                .HasColumnName("Money")
                .IsRequired()
                .HasPrecision(10,2);

            builder.HasOne(thisType => thisType.AppUser)
                .WithOne(relation => relation.UserDetails)
                .HasForeignKey<UserDetails>(thisType => thisType.AppUserId)
                .IsRequired();

            builder.Ignore(prop => prop.Id);
        }
    }
}
