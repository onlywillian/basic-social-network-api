using MySocial.Domain.Models;

namespace MySocial.Domain.Contracts.Repositories
{
    public interface ICommentRepository
    {
        Task<Comment> Add(Comment comment);
    }
}
