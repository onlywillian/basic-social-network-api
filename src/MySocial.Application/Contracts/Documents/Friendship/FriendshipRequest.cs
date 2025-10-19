using System.ComponentModel.DataAnnotations;

namespace MySocial.Application.Contracts.Documents.Friendship
{
    public class FriendshipRequest
    {
        [Required]
        public int SubjectId { get; set; }

        [Required]
        public int FriendId { get; set; }
    }
}
