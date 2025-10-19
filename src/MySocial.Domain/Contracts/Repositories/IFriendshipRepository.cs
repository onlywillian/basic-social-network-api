using MySocial.Domain.Documents;
using MySocial.Domain.Models;

namespace MySocial.Domain.Contracts.Repositories
{
    public interface IFriendshipRepository
    {
        Task<PagedResponse<User>> GetAllByUser(int userId, int pageNumber, int pageSize);

        Task<List<User>> GetAllByUser(int userId);

        Task<PagedResponse<User>> GetAllByUserPending(int userId, int pageNumber, int pageSize);

        Task<Friendship?> Get(int id);

        Task<Friendship?> GetByIds(int subjectId, int friendId);

        Task<Friendship> Add(Friendship friendship);

        Task<Friendship> Accept(Friendship friendship);

        Task<bool> Remove(Friendship friendship);

        Task<bool> ExistsByIds(int subjectId, int friendId);
    }
}
