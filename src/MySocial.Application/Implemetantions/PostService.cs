using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents;
using MySocial.Application.Contracts.Documents.Like;
using MySocial.Application.Contracts.Documents.Post;
using MySocial.Application.Mappers;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Documents;
using MySocial.Domain.Models;

namespace MySocial.Application.Implemetantions
{
    public class PostService : IPostService
    {
        private readonly IPostRepository _postRepository;
        private readonly ILikeRepository _likeRepository;
        private readonly IFriendshipRepository _friendshipRepository;
        private readonly IUserRepository _userRepository;

        public PostService(IPostRepository postRepository, ILikeRepository likeRepository, IFriendshipRepository friendshipRepository, IUserRepository userRepository)
        {
            _postRepository = postRepository;
            _likeRepository = likeRepository;
            _friendshipRepository = friendshipRepository;
            _userRepository = userRepository;
        }

        public async Task<PagedResponse<Post>> GetAll(int userId, int pageNumber, int pageSize)
        {
            var friends = await _friendshipRepository.GetAllByUser(userId);

            var user = await _userRepository.Get(userId);

            friends.Add(user!);

            var posts = await _postRepository.GetAll(friends, pageNumber, pageSize);

            return posts;
        }

        public async Task<PostResponse> Add(PostRequest request)
        {
            var postMapped = request.ToEntity();

            var post = await _postRepository.Add(postMapped);

            return post.ToResponse();
        }

        public async Task<IdResponse> UpdateVisibility(int id, UpdateVisibilityRequest request)
        {
            var response = new IdResponse();

            var post = await _postRepository.Get(id);

            if (post == null)
            {
                response.AddNotification("Post not found");
                return response;
            }

            var postUpdated = await _postRepository.UpdateVisibility(post, request.visibility);

            return postUpdated.ToIdResponse();
        }

        public async Task<IdResponse> Like(int id, LikeRequest request)
        {
            var like = new Like()
            {
                UserId = request.UserId,
                PostId = id
            };

            var likeAdded = await _likeRepository.Add(like);

            return likeAdded.ToIdResponse();
        }

        public async Task<bool> Unlike(int id, LikeRequest request)
        {
            var like = await _likeRepository.GetByIds(id, request.UserId);

            if (like == null)
                return false;

            var likeRemoved = await _likeRepository.Remove(like);

            if (!likeRemoved)
                return false;

            return true;
        }
    }
}
