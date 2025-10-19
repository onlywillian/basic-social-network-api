using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.Friendship;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Documents;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;

namespace MySocial.Application.Implemetantions
{
    public class FriendshipService : IFriendshipService
    {
        private readonly IFriendshipRepository _friendshipRepository;

        public FriendshipService(IFriendshipRepository friendshipRepository)
        {
            _friendshipRepository = friendshipRepository;
        }

        public async Task<Friendship> Accept(int id)
        {
            var friendship = await _friendshipRepository.Get(id);

            if (friendship == null)
                return null;

            await _friendshipRepository.Accept(friendship);

            var friend = new Friendship()
            {
                SubjectId = friendship.FriendId,
                FriendId = friendship.SubjectId,
                Status = Status.ACCEPTED,
            };

            await _friendshipRepository.Add(friend);

            return friendship;
        }

        public async Task<PagedResponse<User>> GetAllByUser(int userId, int pageNumber, int pageSize)
        {
            return await _friendshipRepository.GetAllByUser(userId, pageNumber, pageSize);
        }

        public async Task<PagedResponse<User>> GetAllByUserPending(int userId, int pageNumber, int pageSize)
        {
            return await _friendshipRepository.GetAllByUserPending(userId, pageNumber, pageSize);
        }

        public async Task<bool> Reject(int id)
        {
            var friendship = await _friendshipRepository.Get(id);

            if (friendship == null)
                return false;

            bool friendshipRemoved = await _friendshipRepository.Remove(friendship);

            if (!friendshipRemoved)
                return false;

            var otherFriendship = await _friendshipRepository.GetByIds(friendship.SubjectId, friendship.FriendId);

            if (otherFriendship == null)
                return true;

            await _friendshipRepository.Remove(otherFriendship);

            return true;
        }

        public async Task<Friendship> Request(FriendshipRequest request)
        {
            if (request.SubjectId == request.FriendId)
                return null;

            var isFriends = await _friendshipRepository.ExistsByIds(request.SubjectId, request.FriendId);

            if (isFriends)
                return null;

            var friendshipMapped = new Friendship()
            {
                SubjectId = request.SubjectId,
                FriendId = request.FriendId,
                Status = Status.PENDING,
            };

            var friendship = await _friendshipRepository.Add(friendshipMapped);

            return friendship;
        }
    }
}
