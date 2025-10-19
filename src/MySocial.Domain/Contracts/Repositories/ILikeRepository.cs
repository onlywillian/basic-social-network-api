using MySocial.Domain.Models;

namespace MySocial.Domain.Contracts.Repositories
{
    public interface ILikeRepository
    {
        Task<Like?> GetByIds(int postId, int userId);

        Task<Like> Add(Like like);

        Task<bool> Remove(Like like);
    }
}
