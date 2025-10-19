namespace MySocial.Application.Contracts.Documents.Comment
{
    public class CommentResponse
    {
        public int Id { get; set; }

        public string Message { get; set; } = null!;

        public int PostId { get; set; }

        public int AuthorId { get; set; }

        public DateTime CreatedAt { get; set; }
    }
}
