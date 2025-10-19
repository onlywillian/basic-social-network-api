using MySocial.Application.Contracts.Documents.Comment;
using UserDomain = MySocial.Domain.Models.User;
using MySocial.Application.Validations;
using MySocial.Domain.Enumerators;

namespace MySocial.Application.Contracts.Documents.Post
{
    public class PostResponse : Notifiable
    {
        public int Id { get; set; }

        public string Title { get; set; } = null!;

        public string Body { get; set; } = null!;

        public Visibilitys Visibility { get; set; }

        public string? ImageUrl { get; set; }

        public int AuthorId { get; set; }

        public UserDomain Author { get; set; } = null!;

        public int Comments { get; set; }

        public int Likes { get; set; }
    }
}
