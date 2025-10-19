namespace MySocial.Domain.Models
{
    public class User
    {
        public int Id { get; set; }

        public string Name { get; set; } = null!;

        public string Email { get; set; } = null!;

        public string? Nickname { get; set; }

        public DateOnly BirthDate { get; set; }

        public string Cep { get; set; } = null!;

        public string Password { get; set; } = null!;

        public string? ProfileImage { get; set; }

        public List<Friendship> Friendships { get; set; } = new();

        public List<Post> Posts { get; set; } = new();
    }
}
