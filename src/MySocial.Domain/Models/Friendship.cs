using MySocial.Domain.Enumerators;

namespace MySocial.Domain.Models
{
    public class Friendship
    {
        public int Id { get; set; }

        public int SubjectId { get; set; }

        public User Subject { get; set; } = null!;

        public int FriendId { get; set; }

        public User Friend { get; set; } = null!;

        public Status Status { get; set; }
    }
}
