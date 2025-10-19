using MySocial.Domain.Enumerators;

namespace MySocial.Domain.Models
{
    public class Post
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public Visibilitys Visibility { get; set; }

        public string? ImageUrl { get; set; }

        public DateTime CreatedAt { get; set; }

        public DateTime UpdatedAt { get; set; }

        public int AuthorId { get; set; }

        public User Author { get; set; } = null!;

        public List<Comment> Comments { get; set; } = null!;

        public List<Like> Likes { get; set; } = null!;
    }
}
