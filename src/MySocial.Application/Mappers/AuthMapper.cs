using MySocial.Application.Contracts.Documents.Auth;
using MySocial.Domain.Models;

namespace MySocial.Application.Mappers
{
    public static class AuthMapper
    {
        public static LoginResponse ToLoginResponse(this User entity)
        {
            return new LoginResponse()
            {
                IsAuthenticated = true, 
                Username = entity.Name, 
                Email = entity.Email, 
                Id = entity.Id
            };
        }
    }
}
