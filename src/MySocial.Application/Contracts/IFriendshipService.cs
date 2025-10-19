using MySocial.Application.Contracts.Documents.Friendship;
using MySocial.Domain.Documents;
using MySocial.Domain.Models;

namespace MySocial.Application.Contracts
{
    public interface IFriendshipService
    {
        Task<PagedResponse<User>> GetAllByUser(int userId, int pageNumber, int pageSize);

        Task<PagedResponse<User>> GetAllByUserPending(int userId, int pageNumber, int pageSize);

        Task<Friendship> Request(FriendshipRequest friendship);

        Task<Friendship> Accept(int id);

        Task<bool> Reject(int id);
    }
}
