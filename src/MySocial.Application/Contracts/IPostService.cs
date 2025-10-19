using MySocial.Application.Contracts.Documents;
using MySocial.Application.Contracts.Documents.Like;
using MySocial.Application.Contracts.Documents.Post;
using MySocial.Domain.Documents;
using MySocial.Domain.Models;

namespace MySocial.Application.Contracts
{
    public interface IPostService
    {
        Task<PagedResponse<Post>> GetAll(int userId, int pageNumber, int pageSize);

        Task<PostResponse> Add(PostRequest request);

        Task<IdResponse> UpdateVisibility(int id, UpdateVisibilityRequest request);

        Task<IdResponse> Like(int id, LikeRequest request);

        Task<bool> Unlike(int id, LikeRequest request);
    }
}
