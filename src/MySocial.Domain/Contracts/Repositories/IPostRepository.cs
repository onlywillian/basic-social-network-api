using MySocial.Domain.Documents;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;

namespace MySocial.Domain.Contracts.Repositories
{
    public interface IPostRepository
    {
        Task<PagedResponse<Post>> GetAll(List<User> users, int pageNumber, int pageSize);

        Task<Post?> Get(int id);

        Task<Post> Add(Post post);

        Task<Post> UpdateVisibility(Post post, Visibilitys visibility);
    }
}
