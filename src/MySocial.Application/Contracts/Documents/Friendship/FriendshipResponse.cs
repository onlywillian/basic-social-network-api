using MySocial.Domain.Enumerators;
using DomainUser = MySocial.Domain.Models.User;

namespace MySocial.Application.Contracts.Documents.Friendship
{
    public class FriendshipResponse
    {
        public DomainUser Friend { get; set; } = null!;

        public Status Status { get; set; }
    }
}
