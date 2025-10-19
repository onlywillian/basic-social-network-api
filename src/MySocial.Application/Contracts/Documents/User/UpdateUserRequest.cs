using System.ComponentModel.DataAnnotations;

namespace MySocial.Application.Contracts.Documents.User
{
    public class UpdateUserRequest
    {
        [MaxLength(50)]
        public string Nickname { get; set; }

        [MaxLength(512)]
        public string ProfileImage { get; set; }
    }
}
