using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Sat.Recruitment.Core.Entities;

namespace Sat.Recruitment.Infrastructure.Data.Configurations
{
    public class UserConfiguration : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.ToTable("Users");

            builder.HasKey(e => e.Id);
            builder.HasAlternateKey(c => new { c.Email, c.Phone, c.Address }).HasName("IX_EmailPhoneAddress");

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
        }
    }
}
