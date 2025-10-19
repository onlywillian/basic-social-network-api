namespace MySocial.Application.Contracts.Documents.Comment
{
    public class CommentRequest
    {
        public string Message { get; set; } = null!;

        public int AuthorId { get; set; }
    }
}
