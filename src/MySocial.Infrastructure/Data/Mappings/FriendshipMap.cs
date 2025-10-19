using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;

namespace MySocial.Infrastructure.Data.Mappings
{
    public class FriendshipMap : IEntityTypeConfiguration<Friendship>
    {
        public void Configure(EntityTypeBuilder<Friendship> builder)
        {
            builder.HasKey(x => x.Id);

            builder.HasIndex(x => new { x.SubjectId, x.FriendId }).IsUnique();

            builder.Property(x => x.Id)
               .ValueGeneratedOnAdd();

            builder.Property(x => x.Status)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("status")
                .HasColumnType("VARCHAR")
                .HasConversion(
                    v => v.ToString(),
                    v => (Status)Enum.Parse(typeof(Status), v)
                );

            builder.HasOne(f => f.Subject)
                   .WithMany(u => u.Friendships)
                   .HasForeignKey(x => x.SubjectId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(f => f.Friend)
                   .WithMany()
                   .HasForeignKey(x => x.FriendId)
                   .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
