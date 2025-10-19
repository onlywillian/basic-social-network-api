namespace MySocial.Application.Contracts.Documents.Auth
{
    public class LoginResponse
    {
        public bool IsAuthenticated { get; set; } = false;

        public int Id { get; set; }

        public string Username { get; set; } = null!;

        public string Email { get; set; } = null!;
    }
}
