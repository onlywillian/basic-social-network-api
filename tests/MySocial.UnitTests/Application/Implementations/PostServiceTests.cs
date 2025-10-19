using Moq;
using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.Like;
using MySocial.Application.Contracts.Documents.Post;
using MySocial.Application.Implemetantions;
using MySocial.Application.Mappers;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Documents;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;
using MySocial.UnitTests.Contracts.Documents;

namespace MySocial.UnitTests.Application.Implementations
{
    public class PostServiceTests
    {
        private readonly Mock<IPostRepository> _postRepository = new();
        private readonly Mock<ILikeRepository> _likeRepository = new();
        private readonly Mock<IFriendshipRepository> _friendshipRepository = new();
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly IPostService _postService;
        private readonly UserFakers _userFakers = new();
        private readonly PostFakers _postFakers = new();

        public PostServiceTests()
        {
            _postService = new PostService(_postRepository.Object, _likeRepository.Object, _friendshipRepository.Object, _userRepository.Object);
        }

        [Fact]
        public async Task GetAll_ShouldReturnPostsOfUserAndFriends_WhenFriendsExist()
        {
            // arrange
            int userId = 1;
            int pageNumber = 1;
            int pageSize = 10;

            var user = _userFakers.User
                .RuleFor(p => p.Posts, f => new List<Post>()
                {
                    _postFakers.Post.Generate()
                })
                .Generate();
            var friend1 = _userFakers.User
                .RuleFor(p => p.Posts, f => new List<Post>()
                {
                    _postFakers.Post.Generate()
                })
                .Generate();
            var friend2 = _userFakers.User
                .RuleFor(p => p.Posts, f => new List<Post>()
                {
                    _postFakers.Post.Generate()
                })
                .Generate();

            var friends = new List<User> { friend1, friend2 };
            var posts = new PagedResponse<Post> 
            { 
                Items = new List<Post>()
                {
                    user.Posts[0],
                    friend1.Posts[0],
                    friend2.Posts[0]
                },
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            _userRepository.Setup(u => u.Get(userId)).ReturnsAsync(user);
            _friendshipRepository.Setup(f => f.GetAllByUser(userId)).ReturnsAsync(friends);
            _postRepository.Setup(p => p.GetAll(It.IsAny<List<User>>(), pageNumber, pageSize)).ReturnsAsync(posts);

            // act
            var result = await _postService.GetAll(userId, pageNumber, pageSize);

            // assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Items.Count());
            _friendshipRepository.Verify(f => f.GetAllByUser(userId), Times.Once);
            _postRepository.Verify(p => p.GetAll(It.Is<List<User>>(u => u.Count == 3 && u.Contains(user) && u.Contains(friend1) && u.Contains(friend2)), pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async Task Add_ShouldReturnPostResponse_WhenPostIsAddedSuccessfully()
        {
            // arrange
            var postRequest = _postFakers.PostRequest.Generate();

            var postMapped = postRequest.ToEntity();
            var post = _postFakers.Post.Generate();

            _postRepository
                .Setup(p => p.Add(It.IsAny<Post>()))
                .ReturnsAsync(post);

            // act
            var response = await _postService.Add(postRequest);

            // assert
            Assert.NotNull(response);
            Assert.Equal(post.Id, response.Id);
            Assert.Equal(post.Body, response.Body);

            _postRepository.Verify(p => p.Add(It.IsAny<Post>()), Times.Once);
        }

        [Fact]
        public async Task UpdateVisibility_ShouldReturnIdResponse_WhenPostIsUpdatedSuccessfully()
        {
            // arrange
            var post = _postFakers.Post.Generate();
            var request = new UpdateVisibilityRequest { visibility = Visibilitys.PUBLIC };

            _postRepository
                .Setup(p => p.Get(It.IsAny<int>()))
                .ReturnsAsync(post); 

            _postRepository
                .Setup(p => p.UpdateVisibility(It.IsAny<Post>(), It.IsAny<Visibilitys>()))
                .ReturnsAsync(post);

            // act
            var response = await _postService.UpdateVisibility(post.Id, request);

            // assert
            Assert.NotNull(response); 
            Assert.Equal(post.Id, response.Id); 
            _postRepository.Verify(p => p.UpdateVisibility(post, request.visibility), Times.Once);
        }

        [Fact]
        public async Task UpdateVisibility_ShouldReturnNotification_WhenPostNotFound()
        {
            // arrange
            var request = new UpdateVisibilityRequest { visibility = Visibilitys.PUBLIC }; 

            _postRepository
                .Setup(p => p.Get(It.IsAny<int>()))
                .ReturnsAsync((Post) null); 

            // act
            var response = await _postService.UpdateVisibility(1, request);

            // assert
            Assert.NotNull(response); 
            Assert.Contains("Post not found", response.Notifications.Select(n => n.Message));
            _postRepository.Verify(p => p.UpdateVisibility(It.IsAny<Post>(), It.IsAny<Visibilitys>()), Times.Never);
        }

        [Fact]
        public async Task Like_ShouldReturnIdResponse_WhenLikeIsAddedSuccessfully()
        {
            // arrange
            var likeRequest = new LikeRequest { UserId = 1 };
            var like = new Like { UserId = likeRequest.UserId, PostId = 1 }; 

            _likeRepository
                .Setup(l => l.Add(It.IsAny<Like>()))
                .ReturnsAsync(like); 

            // act
            var response = await _postService.Like(1, likeRequest);

            // assert
            Assert.NotNull(response);
            _likeRepository.Verify(l => l.Add(It.IsAny<Like>()), Times.Once); 
        }

        [Fact]
        public async Task Unlike_ShouldReturnTrue_WhenLikeIsRemovedSuccessfully()
        {
            // arrange
            var likeRequest = new LikeRequest { UserId = 1 }; 
            var like = new Like { PostId = 1, UserId = likeRequest.UserId }; 

            _likeRepository
                .Setup(l => l.GetByIds(like.PostId, likeRequest.UserId))
                .ReturnsAsync(like); 

            _likeRepository
                .Setup(l => l.Remove(like))
                .ReturnsAsync(true);

            // act
            var result = await _postService.Unlike(like.PostId, likeRequest);

            // assert
            Assert.True(result);
            _likeRepository.Verify(l => l.GetByIds(like.PostId, likeRequest.UserId), Times.Once);
            _likeRepository.Verify(l => l.Remove(like), Times.Once); 
        }

        [Fact]
        public async Task Unlike_ShouldReturnFalse_WhenLikeDoesNotExist()
        {
            // arrange
            var likeRequest = new LikeRequest { UserId = 1 };

            _likeRepository
                .Setup(l => l.GetByIds(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync((Like?)null); 

            // act
            var result = await _postService.Unlike(1, likeRequest);

            // assert
            Assert.False(result); 
            _likeRepository.Verify(l => l.GetByIds(It.IsAny<int>(), It.IsAny<int>()), Times.Once); 
            _likeRepository.Verify(l => l.Remove(It.IsAny<Like>()), Times.Never); 
        }

        [Fact]
        public async Task Unlike_ShouldReturnFalse_WhenLikeRemovalFails()
        {
            // arrange
            var likeRequest = new LikeRequest { UserId = 1 };
            var like = new Like { PostId = 1, UserId = likeRequest.UserId };

            _likeRepository
                .Setup(l => l.GetByIds(like.PostId, likeRequest.UserId))
                .ReturnsAsync(like);

            _likeRepository
                .Setup(l => l.Remove(like))
                .ReturnsAsync(false); 

            // act
            var result = await _postService.Unlike(like.PostId, likeRequest);

            // assert
            Assert.False(result); 
            _likeRepository.Verify(l => l.GetByIds(like.PostId, likeRequest.UserId), Times.Once); 
            _likeRepository.Verify(l => l.Remove(like), Times.Once);
        }
    }
}
