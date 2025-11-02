using System.Net;

namespace FunctionalTests.Support
{
    [Binding]
    public class TestContext
    {
        public HttpResponseMessage? Response { get; set; }
        public HttpStatusCode? StatusCode => Response?.StatusCode;
        public string? ResponseBody { get; set; }
        public string? Token { get; set; }
        public LoginRequest? LoginRequest { get; set; }
    }

    public class LoginRequest
    {
        public string Email { get; set; } = null!;
        public string Password { get; set; } = null!;
    }

    public class LoginResponse
    {
        public User? User { get; set; }
        public string? Token { get; set; }
    }

    public class User
    {
        public int Id { get; set; }
        public string Username { get; set; } = null!;
        public string Email { get; set; } = null!;
        public bool IsAuthenticated { get; set; }
    }
}

