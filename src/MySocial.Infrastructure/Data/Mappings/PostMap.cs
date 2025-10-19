using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;

namespace MySocial.Infrastructure.Data.Mappings
{
    public class PostMap : IEntityTypeConfiguration<Post>
    {
        public void Configure(EntityTypeBuilder<Post> builder)
        {
            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .ValueGeneratedOnAdd();

            builder.Property(x => x.Title)
                .IsRequired()
                .HasMaxLength(200)
                .HasColumnName("title")
                .HasColumnType("VARCHAR");

            builder.Property(x => x.Body)
                .IsRequired()
                .HasColumnName("body")
                .HasColumnType("TEXT");

            builder.Property(x => x.Visibility)
                .IsRequired()
                .HasMaxLength(50)
                .HasColumnName("visibility")
                .HasColumnType("VARCHAR")
                .HasConversion(
                    v => v.ToString(),
                    v => (Visibilitys)Enum.Parse(typeof(Visibilitys), v)
                );

            builder.Property(x => x.ImageUrl)
                .IsRequired(false)
                .HasMaxLength(512)
                .HasColumnName("image_url")
                .HasColumnType("VARCHAR");

            builder.Property(x => x.CreatedAt)
                .IsRequired()
                .HasColumnName("created_at")
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("CURRENT_TIMESTAMP");

            builder.Property(x => x.UpdatedAt)
                .IsRequired()
                .HasColumnName("updated_at")
                .HasColumnType("TIMESTAMP")
                .HasDefaultValueSql("CURRENT_TIMESTAMP")
                .ValueGeneratedOnUpdate();

            builder.HasOne(x => x.Author)
                .WithMany(x => x.Posts)
                .HasForeignKey(x => x.AuthorId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
