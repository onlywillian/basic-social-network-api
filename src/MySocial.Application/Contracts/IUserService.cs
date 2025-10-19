using MySocial.Application.Contracts.Documents.Auth;
using MySocial.Application.Contracts.Documents.User;
using MySocial.Domain.Models;

namespace MySocial.Application.Contracts
{
    public interface IUserService
    {
        Task<User?> Get(int loginId, int userId);

        Task<UserResponse> Add(UserRequest request);

        Task<UserResponse> Update(int id, UpdateUserRequest newUser);

        Task<IEnumerable<User>> GetAllByNameOrEmail(string search);
    }
}
