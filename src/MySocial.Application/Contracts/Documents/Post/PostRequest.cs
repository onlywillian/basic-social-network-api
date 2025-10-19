using MySocial.Domain.Enumerators;
using System.ComponentModel.DataAnnotations;

namespace MySocial.Application.Contracts.Documents.Post
{
    public class PostRequest
    {
        [Required]
        [MaxLength(200)]
        public string Title { get; set; } = null!;

        [Required]
        [MaxLength(1000)]
        public string Body { get; set; } = null!;

        [Required]
        [EnumDataType(typeof(Visibilitys))]
        public Visibilitys Visibility { get; set; }

        public string? ImageUrl { get; set; }

        public int AuthorId { get; set; }
    }
}
