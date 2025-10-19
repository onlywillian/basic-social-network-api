using MySocial.Domain.Models;

namespace MySocial.Domain.Contracts.Repositories
{
    public interface IUserRepository
    {
        Task<User?> Get(int id);

        Task<User?> GetWithPublicPosts(int id);

        Task<User?> GetByEmailAndPassword(string email, string password);

        Task<IEnumerable<User>> GetAllByNameOrEmail(string search);

        Task<bool> ExistsByEmail(string email);

        Task<User> Add(User user);

        Task<User> Update(User user, string nickname, string profileImage);
    }
}
