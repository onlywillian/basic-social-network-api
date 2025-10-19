using Moq;
using MySocial.Application.Contracts;
using MySocial.Application.Implemetantions;
using MySocial.Domain.Contracts.Repositories;
using MySocial.Domain.Models;
using MySocial.UnitTests.Contracts.Documents;

namespace MySocial.UnitTests.Application.Implementations
{
    public class AuthServiceTests
    {
        private readonly Mock<IUserRepository> _userRepository = new();
        private readonly IAuthService _authService;
        private readonly UserFakers _userFakers = new();

        public AuthServiceTests()
        {
            _authService = new AuthService(_userRepository.Object);
        }

        [Fact]
        public async void ValidateLogin_ShouldReturnSuccess_WhenEmailAndPasswordCorrect()
        {
            // arrange
            var loginRequest = _userFakers.LoginRequest.Generate();

            var _user = _userFakers.User.Generate();

            _userRepository
                .Setup(s => s.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync(_user);

            //act
            var response = await _authService.ValidateLogin(loginRequest);

            //assert
            Assert.True(response.IsAuthenticated);
            Assert.Equal(response.Username, _user.Name);
            Assert.Equal(response.Email, _user.Email);
            _userRepository.Verify(s => s.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async void ValidateLogin_ShouldReturnEmptyResponse_WhenEmailAndPasswordIncorrect()
        {
            // arrange
            var loginRequest = _userFakers.LoginRequest.Generate();

            var _user = _userFakers.User.Generate();

            _userRepository
                .Setup(s => s.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()))
                .ReturnsAsync((User?)null);

            //act
            var response = await _authService.ValidateLogin(loginRequest);

            //assert
            Assert.False(response.IsAuthenticated);
            Assert.Null(response.Username);
            Assert.Null(response.Email);
            _userRepository.Verify(s => s.GetByEmailAndPassword(It.IsAny<string>(), It.IsAny<string>()), Times.Once);
        }
    }
}
