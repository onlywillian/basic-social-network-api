using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySocial.Domain.Models;

namespace MySocial.Infrastructure.Data.Mappings
{
    public class UserMap : IEntityTypeConfiguration<User>
    {
        public void Configure(EntityTypeBuilder<User> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Name)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("name")
                .HasColumnType("VARCHAR");

            builder.Property(x => x.Email)
                .IsRequired()
                .HasMaxLength(255)
                .HasColumnName("email")
                .HasColumnType("VARCHAR");

            builder.Property(x => x.Nickname)
                .IsRequired(false)
                .HasMaxLength(50)
                .HasColumnName("nickname")
                .HasColumnType("VARCHAR");

            builder.Property(x => x.BirthDate)
                .IsRequired()
                .HasColumnName("nickname")
                .HasColumnType("DATE");

            builder.Property(x => x.BirthDate)
                .IsRequired()
                .HasMaxLength(8)
                .HasColumnName("birthdate")
                .HasColumnType("VARCHAR");

            builder.Property(x => x.Password)
                .IsRequired()
                .HasMaxLength(128)
                .HasColumnName("password")
                .HasColumnType("VARCHAR");

            builder.Property(x => x.ProfileImage)
                .IsRequired(false)
                .HasMaxLength(512)
                .HasColumnName("profile_image")
                .HasColumnType("VARCHAR");

            builder.HasIndex(x => x.Email)
                .IsUnique();
        }
    }
}
