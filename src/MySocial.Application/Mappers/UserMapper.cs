using MySocial.Application.Contracts.Documents.User;
using MySocial.Domain.Models;
using MySocial.Domain.Utils;

namespace MySocial.Application.Mappers
{
    public static class UserMapper
    {
        public static User ToEntity(this UserRequest request)
        {
            return new User()
            {
                Name = request.Name,
                Email = request.Email,
                Password = PasswordHash.Hash(request.Password),
                BirthDate = request.BirthDate,
                Nickname = request.Nickname,
                Cep = request.Cep,
                ProfileImage = request.ProfileImage,
            };
        }

        public static UserResponse ToResponse(this User entity)
        {
            return new UserResponse()
            {
                Name = entity.Name,
                Email = entity.Email,
                Nickname = entity.Nickname,
                ProfileImage = entity.ProfileImage,
            };
        }
    }
}
