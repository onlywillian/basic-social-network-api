using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.Auth;
using MySocial.Application.Mappers;
using MySocial.Domain.Contracts.Repositories;

namespace MySocial.Application.Implemetantions
{
    public class AuthService : IAuthService
    {
        private readonly IUserRepository _userRepository;

        public AuthService(IUserRepository userRepository)
        {
            _userRepository = userRepository;
        }

        public async Task<LoginResponse> ValidateLogin(LoginRequest loginRequest)
        {
            var user = await _userRepository.GetByEmailAndPassword(loginRequest.Email, loginRequest.Password);

            if (user == null)
            {
                return new LoginResponse();
            }

            return user.ToLoginResponse();
        }
    }
}
