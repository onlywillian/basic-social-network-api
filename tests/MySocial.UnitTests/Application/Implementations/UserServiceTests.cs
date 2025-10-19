using Moq;
using MySocial.Application.Contracts;
using MySocial.Application.Contracts.Documents.User;
using MySocial.Application.Implemetantions;
using MySocial.Application.Mappers;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Models;
using MySocial.UnitTests.Contracts.Documents;

namespace MySocial.UnitTests.Application.Implementations
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly Mock<IFriendshipRepository> _friendshipRepository = new();
        private readonly IUserService _userService;
        private readonly UserFakers _userFakers = new();

        public UserServiceTests()
        {
            _userService = new UserService(_userRepository.Object, _friendshipRepository.Object);
        }

        [Fact]
        public async void Get_ShouldReturnUser_WhenLoginIdEqualUserId()
        {
            // arrange
            int loginId = 1;
            int userId = 1;
            var user = _userFakers.User.Generate();

            _userRepository.Setup(r => r.Get(userId)).ReturnsAsync(user);

            // act
            var result = await _userService.Get(loginId, userId);

            // assert
            Assert.Equal(user, result);
            _userRepository.Verify(r => r.Get(userId), Times.Once);
            _friendshipRepository.Verify(f => f.ExistsByIds(It.IsAny<int>(), It.IsAny<int>()), Times.Never);
        }

        [Fact]
        public async Task Get_ShouldReturnUser_WhenUsersAreFriends()
        {
            // arrange
            int loginId = 1;
            int userId = 2;
            var user = _userFakers.User.Generate();

            _friendshipRepository.Setup(f => f.ExistsByIds(loginId, userId)).ReturnsAsync(true);
            _userRepository.Setup(r => r.Get(userId)).ReturnsAsync(user);

            // act
            var result = await _userService.Get(loginId, userId);

            // assert
            Assert.Equal(user, result);
            _friendshipRepository.Verify(f => f.ExistsByIds(loginId, userId), Times.Once);
            _userRepository.Verify(r => r.Get(userId), Times.Once);
            _userRepository.Verify(r => r.GetWithPublicPosts(userId), Times.Never);
        }

        [Fact]
        public async Task Get_ShouldReturnUserWithPublicPosts_WhenUsersAreNotFriends()
        {
            // arrange
            int loginId = 1;
            int userId = 2;
            var userWithPublicPosts = _userFakers.User.Generate();

            _friendshipRepository.Setup(f => f.ExistsByIds(loginId, userId)).ReturnsAsync(false);
            _userRepository.Setup(r => r.GetWithPublicPosts(userId)).ReturnsAsync(userWithPublicPosts);

            // act
            var result = await _userService.Get(loginId, userId);

            // assert
            Assert.Equal(userWithPublicPosts, result);
            _friendshipRepository.Verify(f => f.ExistsByIds(loginId, userId), Times.Once);
            _userRepository.Verify(r => r.GetWithPublicPosts(userId), Times.Once);
            _userRepository.Verify(r => r.Get(userId), Times.Never);
        }

        [Fact]
        public async void Add_ShouldReturnNotification_WhenEmailFormatIsInvalid()
        {
            // arrange
            var userRequest = _userFakers.UserRequest
                .RuleFor(p =>  p.Email, f => "invalid-email")
                .Generate();

            // act
            var response = await _userService.Add(userRequest);

            // assert
            Assert.NotNull(response);
            Assert.True(response.HasNotifications);

            _userRepository.Verify(r => r.ExistsByEmail(It.IsAny<string>()), Times.Never);
            _userRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task Add_ShouldReturnNotification_WhenEmailAlreadyExists()
        {
            // arrange
            var userRequest = _userFakers.UserRequest
                .RuleFor(p => p.Email, f => "existing-email@example.com")
                .Generate();

            _userRepository.Setup(r => r.ExistsByEmail(userRequest.Email)).ReturnsAsync(true);

            // act
            var response = await _userService.Add(userRequest);

            // assert
            Assert.NotNull(response);
            Assert.True(response.HasNotifications);

            _userRepository.Verify(r => r.ExistsByEmail(userRequest.Email), Times.Once);
            _userRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async void Add_ShouldAddUser_WhenEmailIsValidAndDoesNotExist()
        {
            // arrange
            var userRequest = _userFakers.UserRequest.Generate();

            var userMapped = userRequest.ToEntity();
            var userResponse = userMapped.ToResponse();

            _userRepository.Setup(r => r.ExistsByEmail(userRequest.Email)).ReturnsAsync(false);
            _userRepository.Setup(r => r.Add(It.IsAny<User>())).ReturnsAsync(userMapped);

            // act
            var response = await _userService.Add(userRequest);

            // assert
            Assert.NotNull(response);
            Assert.False(response.HasNotifications);
            Assert.Equal(userResponse.Email, response.Email);

            _userRepository.Verify(r => r.ExistsByEmail(userRequest.Email), Times.Once);
            _userRepository.Verify(r => r.Add(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task Update_ShouldReturnNotification_WhenUserNotFound()
        {
            // arrange
            int userId = 1;
            var updateRequest = new UpdateUserRequest
            {
                Nickname = "New Nickname",
                ProfileImage = "new_image_url.jpg"
            };

            _userRepository.Setup(r => r.Get(userId)).ReturnsAsync((User?)null);

            // act
            var response = await _userService.Update(userId, updateRequest);

            // assert
            Assert.NotNull(response);
            Assert.True(response.HasNotifications);

            _userRepository.Verify(r => r.Get(userId), Times.Once);
            _userRepository.Verify(r => r.Update(It.IsAny<User>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task Update_ShouldUpdateUser_WhenUserIsFound()
        {
            // arrange
            int userId = 1;
            var user = _userFakers.User.Generate();
            var updateRequest = new UpdateUserRequest
            {
                Nickname = "Updated Nickname",
                ProfileImage = "updated_image_url.jpg"
            };

            var updatedUser = user;
            updatedUser.Nickname = updateRequest.Nickname;
            updatedUser.ProfileImage = updateRequest.ProfileImage;

            _userRepository.Setup(r => r.Get(userId)).ReturnsAsync(user);
            _userRepository.Setup(r => r.Update(user, updateRequest.Nickname, updateRequest.ProfileImage)).ReturnsAsync(updatedUser);

            // act
            var response = await _userService.Update(userId, updateRequest);

            // assert
            Assert.NotNull(response);
            Assert.False(response.HasNotifications);
            Assert.Equal(updatedUser.Nickname, response.Nickname);
            Assert.Equal(updatedUser.ProfileImage, response.ProfileImage);

            _userRepository.Verify(r => r.Get(userId), Times.Once);
            _userRepository.Verify(r => r.Update(user, updateRequest.Nickname, updateRequest.ProfileImage), Times.Once);
        }

        [Fact]
        public async Task GetAllByNameOrEmail_ShouldReturnUsers_WhenSearchMatches()
        {
            // arrange
            string search = "john";
            var users = _userFakers.User.Generate(3);

            _userRepository
                .Setup(r => r.GetAllByNameOrEmail(search.ToLower()))
                .ReturnsAsync(users);

            // act
            var result = await _userService.GetAllByNameOrEmail(search);

            // assert
            Assert.NotNull(result);
            Assert.Equal(3, result.Count());
            _userRepository.Verify(r => r.GetAllByNameOrEmail(search.ToLower()), Times.Once);
        }

        [Fact]
        public async Task GetAllByNameOrEmail_ShouldReturnEmptyList_WhenNoUserMatches()
        {
            // arrange
            string search = "nonexistent";
            var users = new List<User>();

            _userRepository
                .Setup(r => r.GetAllByNameOrEmail(search.ToLower()))
                .ReturnsAsync(users);

            // act
            var result = await _userService.GetAllByNameOrEmail(search);

            // assert
            Assert.NotNull(result);
            Assert.Empty(result);
            _userRepository.Verify(r => r.GetAllByNameOrEmail(search.ToLower()), Times.Once);
        }
    }
}