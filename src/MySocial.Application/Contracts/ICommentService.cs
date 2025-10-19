using MySocial.Application.Contracts.Documents.Comment;
using MySocial.Domain.Models;

namespace MySocial.Application.Contracts
{
    public interface ICommentService
    {
        Task<Comment> Add(int id, CommentRequest request);
    }
}
