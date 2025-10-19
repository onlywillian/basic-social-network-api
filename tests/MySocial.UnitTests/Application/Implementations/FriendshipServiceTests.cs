using Moq;
using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.Friendship;
using MySocial.Application.Implemetantions;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Documents;
using MySocial.Domain.Enumerators;
using MySocial.Domain.Models;
using MySocial.UnitTests.Contracts.Documents;

namespace MySocial.UnitTests.Application.Implementations
{
    public class FriendshipServiceTests
    {
        private readonly Mock<IFriendshipRepository> _friendshipRepository = new();
        private readonly IFriendshipService _friendshipService;
        private readonly FriendshipFakers _friendshipFakers = new();
        private readonly UserFakers _userFakers = new();

        public FriendshipServiceTests()
        {
            _friendshipService = new FriendshipService(_friendshipRepository.Object);
        }

        [Fact]
        public async void Accept_ShouldReturnNull_WhenFriendshipNotFound()
        {
            // arrange
            int friendshipId = 1;

            _friendshipRepository.Setup(r => r.Get(friendshipId)).ReturnsAsync((Friendship?)null);

            // act
            var result = await _friendshipService.Accept(friendshipId);

            // assert
            Assert.Null(result);
            _friendshipRepository.Verify(r => r.Get(friendshipId), Times.Once);
            _friendshipRepository.Verify(r => r.Accept(It.IsAny<Friendship>()), Times.Never);
            _friendshipRepository.Verify(r => r.Add(It.IsAny<Friendship>()), Times.Never);
        }

        [Fact]
        public async void Accept_ShouldAcceptFriendship_WhenFriendshipIsFound()
        {
            // arrange
            int friendshipId = 1;
            var friendship = _friendshipFakers.Friendship.Generate();

            _friendshipRepository.Setup(r => r.Get(friendshipId)).ReturnsAsync(friendship);

            // act
            var result = await _friendshipService.Accept(friendshipId);

            // assert
            Assert.NotNull(result);
            Assert.Equal(friendship.Id, result.Id);

            _friendshipRepository.Verify(r => r.Get(friendshipId), Times.Once);
            _friendshipRepository.Verify(r => r.Accept(friendship), Times.Once);
            _friendshipRepository.Verify(r => r.Add(It.IsAny<Friendship>()), Times.Once);
        }

        [Fact]
        public async void GetAllByUser_ShouldReturnPagedResponse_WhenUsersExist()
        {
            // arrange
            int userId = 1;
            int pageNumber = 1;
            int pageSize = 10;
            var users = _userFakers.User.Generate(5);
            var pagedResponse = new PagedResponse<User>()
            {
                Items = users,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            _friendshipRepository.Setup(r => r.GetAllByUser(userId, pageNumber, pageSize))
                .ReturnsAsync(pagedResponse);

            // act
            var result = await _friendshipService.GetAllByUser(userId, pageNumber, pageSize);

            // assert
            Assert.NotNull(result);
            Assert.Equal(5, result.Items.Count());
            Assert.Equal(pageNumber, result.PageNumber);
            Assert.Equal(pageSize, result.PageSize);

            _friendshipRepository.Verify(r => r.GetAllByUser(userId, pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async void GetAllByUser_ShouldReturnEmptyPagedResponse_WhenNoUsersExist()
        {
            // arrange
            int userId = 1;
            int pageNumber = 1;
            int pageSize = 10;
            var emptyPagedResponse = new PagedResponse<User>()
            {
                Items = new List<User>(),
                PageNumber = pageNumber,
                PageSize = pageSize
            };
            

            _friendshipRepository.Setup(r => r.GetAllByUser(userId, pageNumber, pageSize))
                .ReturnsAsync(emptyPagedResponse);

            // act
            var result = await _friendshipService.GetAllByUser(userId, pageNumber, pageSize);

            // assert
            Assert.NotNull(result);
            Assert.Empty(result.Items);
            Assert.Equal(pageNumber, result.PageNumber);
            Assert.Equal(pageSize, result.PageSize);

            _friendshipRepository.Verify(r => r.GetAllByUser(userId, pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async void GetAllByUserPending_ShouldReturnPagedResponse_WhenPendingUsersExist()
        {
            // arrange
            int userId = 1;
            int pageNumber = 1;
            int pageSize = 10;
            var pendingUsers = _userFakers.User.Generate(3); // Gera uma lista de 3 usuários pendentes
            var pagedResponse = new PagedResponse<User>()
            {
                Items = pendingUsers, PageNumber = pageNumber, PageSize = pageSize
            };

            _friendshipRepository.Setup(r => r.GetAllByUserPending(userId, pageNumber, pageSize))
                .ReturnsAsync(pagedResponse);

            // act
            var result = await _friendshipService.GetAllByUserPending(userId, pageNumber, pageSize);

            // assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Items.Count());
            Assert.Equal(pageNumber, result.PageNumber);
            Assert.Equal(pageSize, result.PageSize);

            _friendshipRepository.Verify(r => r.GetAllByUserPending(userId, pageNumber, pageSize), Times.Once);
        }

        [Fact]
        public async void Reject_ShouldReturnFalse_WhenFriendshipNotFound()
        {
            // arrange
            int friendshipId = 1;

            _friendshipRepository.Setup(r => r.Get(friendshipId))
                .ReturnsAsync((Friendship)null);

            // act
            var result = await _friendshipService.Reject(friendshipId);

            // assert
            Assert.False(result);
            _friendshipRepository.Verify(r => r.Get(friendshipId), Times.Once);
        }

        [Fact]
        public async void Reject_ShouldReturnFalse_WhenFriendshipRemoveFails()
        {
            // arrange
            int friendshipId = 1;
            var friendship = new Friendship { Id = friendshipId };

            _friendshipRepository.Setup(r => r.Get(friendshipId))
                .ReturnsAsync(friendship);
            _friendshipRepository.Setup(r => r.Remove(friendship))
                .ReturnsAsync(false);

            // act
            var result = await _friendshipService.Reject(friendshipId);

            // assert
            Assert.False(result);
            _friendshipRepository.Verify(r => r.Remove(friendship), Times.Once);
        }

        [Fact]
        public async void Reject_ShouldReturnTrue_WhenFriendshipRemovedAndNoInverseFriendship()
        {
            // arrange
            int friendshipId = 1;
            var friendship = new Friendship { Id = friendshipId, SubjectId = 1, FriendId = 2 };

            _friendshipRepository.Setup(r => r.Get(friendshipId))
                .ReturnsAsync(friendship);
            _friendshipRepository.Setup(r => r.Remove(friendship))
                .ReturnsAsync(true);
            _friendshipRepository.Setup(r => r.GetByIds(friendship.FriendId, friendship.SubjectId))
                .ReturnsAsync((Friendship?) null);

            // act
            var result = await _friendshipService.Reject(friendshipId);

            // assert
            Assert.True(result);
            _friendshipRepository.Verify(r => r.Remove(friendship), Times.Once);
            _friendshipRepository.Verify(r => r.GetByIds(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async void Reject_ShouldReturnTrue_WhenFriendshipAndInverseFriendshipRemoved()
        {
            // arrange
            int friendshipId = 1;
            var friendship = new Friendship { Id = friendshipId, SubjectId = 1, FriendId = 2 };
            var inverseFriendship = new Friendship { Id = 2, SubjectId = 2, FriendId = 1 };

            _friendshipRepository.Setup(r => r.Get(It.IsAny<int>()))
                .ReturnsAsync(friendship);
            _friendshipRepository.Setup(r => r.Remove(friendship))
                .ReturnsAsync(true);
            _friendshipRepository.Setup(r => r.GetByIds(It.IsAny<int>(), It.IsAny<int>()))
                .ReturnsAsync(inverseFriendship);
            _friendshipRepository.Setup(r => r.Remove(inverseFriendship))
                .ReturnsAsync(true);

            // act
            var result = await _friendshipService.Reject(friendshipId);

            // assert
            Assert.True(result);
            _friendshipRepository.Verify(r => r.Remove(friendship), Times.Once);
            _friendshipRepository.Verify(r => r.GetByIds(It.IsAny<int>(), It.IsAny<int>()), Times.Once);
            _friendshipRepository.Verify(r => r.Remove(inverseFriendship), Times.Once);
        }

        [Fact]
        public async void Request_ShouldReturnNull_WhenRequestingFriendshipToSelf()
        {
            // arrange
            var request = new FriendshipRequest()
            {
                SubjectId = 1,
                FriendId = 1
            };

            // act
            var result = await _friendshipService.Request(request);

            // assert
            Assert.Null(result);
            _friendshipRepository.Verify(r => r.ExistsByIds(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
            _friendshipRepository.Verify(r => r.Add(It.IsAny<Friendship>()), Times.Never);
        }

        [Fact]
        public async void Request_ShouldReturnNull_WhenFriendshipAlreadyExists()
        {
            // arrange
            var request = new FriendshipRequest()
            {
                SubjectId = 1,
                FriendId = 2
            };

            _friendshipRepository.Setup(r => r.ExistsByIds(request.SubjectId, request.FriendId))
                .ReturnsAsync(true);

            // act
            var result = await _friendshipService.Request(request);

            // assert
            Assert.Null(result);
            _friendshipRepository.Verify(r => r.ExistsByIds(request.SubjectId, request.FriendId), Times.Once);
            _friendshipRepository.Verify(r => r.Add(It.IsAny<Friendship>()), Times.Never);
        }

        [Fact]
        public async void Request_ShouldReturnFriendship_WhenFriendshipIsCreatedSuccessfully()
        {
            // arrange
            var request = new FriendshipRequest()
            {
                SubjectId = 1,
                FriendId = 2
            };

            var newFriendship = new Friendship()
            {
                SubjectId = request.SubjectId,
                FriendId = request.FriendId,
                Status = Status.PENDING
            };

            _friendshipRepository.Setup(r => r.ExistsByIds(request.SubjectId, request.FriendId))
                .ReturnsAsync(false);
            _friendshipRepository.Setup(r => r.Add(It.IsAny<Friendship>()))
                .ReturnsAsync(newFriendship);

            // act
            var result = await _friendshipService.Request(request);

            // assert
            Assert.NotNull(result);
            Assert.Equal(request.SubjectId, result.SubjectId);
            Assert.Equal(request.FriendId, result.FriendId);
            Assert.Equal(Status.PENDING, result.Status);
            _friendshipRepository.Verify(r => r.ExistsByIds(request.SubjectId, request.FriendId), Times.Once);
            _friendshipRepository.Verify(r => r.Add(It.IsAny<Friendship>()), Times.Once);
        }
    }
}
