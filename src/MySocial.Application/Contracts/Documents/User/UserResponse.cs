using MySocial.Application.Validations;

namespace MySocial.Application.Contracts.Documents.User
{
    public class UserResponse : Notifiable
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Nickname { get; set; }

        public string? ProfileImage { get; set; }
    }
}
