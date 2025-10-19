using MySocial.Application.Contracts.Documents.Auth;

namespace MySocial.Application.Contracts
{
    public interface IAuthService
    {
        Task<LoginResponse> ValidateLogin(LoginRequest loginRequest);
    }
}
