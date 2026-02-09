using Moq;
using MoviesApi.DTOs.Auth;
using MoviesApi.DTOs.User.Jwt;
using MoviesApi.Entities;
using MoviesApi.Exceptions;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;
using MoviesApi.Services;
using Xunit;

namespace MovieApiTest.Services
{
    public class AuthServiceTests
    {
        private readonly Mock<IJwtTokenService> _tokenServiceMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IUserMapping> _userMappingMock;

        private readonly AuthService _authService;

        public AuthServiceTests()
        {
            _tokenServiceMock = new Mock<IJwtTokenService>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _userMappingMock = new Mock<IUserMapping>();

            _authService = new AuthService(
                _tokenServiceMock.Object,
                _userRepositoryMock.Object,
                _userMappingMock.Object);
        }

        private AuthLoginRequest LoginRequest(
            string email = "user@test.com",
            string password = "123")
            => new()
            {
                Email = email,
                Password = password
            };

        private AuthRegisterRequest RegisterRequest(
            string email = "user@test.com",
            string password = "123",
            string confirmPassword = "123")
            => new()
            {
                Email = email,
                Password = password,
                ConfirmPassword = confirmPassword
            };

        private User ValidUser(
            string email = "user@test.com",
            string password = "123")
            => new()
            {
                Id = 1,
                Email = email,
                Password = BCrypt.Net.BCrypt.HashPassword(password)
            };

        private UserJwt ValidJwtUser(User user)
            => new()
            {
                Id = user.Id,
                Email = user.Email
            };


        [Fact]
        public async Task LoginAsync_WhenEmailDoesNotExist_ShouldThrowException()
        {
            var request = LoginRequest();

            _userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(request.Email))
                .ReturnsAsync((User)null);

            await Assert.ThrowsAsync<EmailNotFoundException>(() =>
                _authService.LoginAsync(request));
        }

        [Fact]
        public async Task LoginAsync_WhenPasswordIsInvalid_ShouldThrowException()
        {
            var request = LoginRequest();
            var user = ValidUser(password: "correct-password");

            _userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(request.Email))
                .ReturnsAsync(user);

            await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
                _authService.LoginAsync(request));
        }

        [Fact]
        public async Task LoginAsync_WhenValid_ShouldReturnToken()
        {
            var request = LoginRequest();
            var user = ValidUser();
            var jwtUser = ValidJwtUser(user);

            _userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(request.Email))
                .ReturnsAsync(user);

            _userMappingMock
                .Setup(m => m.ToJwtEntity(user))
                .Returns(jwtUser);

            _tokenServiceMock
                .Setup(t => t.GenerateToken(jwtUser))
                .Returns("jwt-token");

            var token = await _authService.LoginAsync(request);

            Assert.Equal("jwt-token", token);
        }

        [Fact]
        public async Task RegisterAsync_WhenEmailAlreadyExists_ShouldThrowException()
        {
            var request = RegisterRequest();

            _userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(request.Email))
                .ReturnsAsync(ValidUser());

            await Assert.ThrowsAsync<EmailAlreadyExistsException>(() =>
                _authService.RegisterAsync(request));
        }

        [Fact]
        public async Task RegisterAsync_WhenPasswordsDoNotMatch_ShouldThrowException()
        {
            var request = RegisterRequest(
                password: "123",
                confirmPassword: "456");

            _userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(request.Email))
                .ReturnsAsync((User)null);

            await Assert.ThrowsAsync<InvalidCredentialsException>(() =>
                _authService.RegisterAsync(request));
        }

        [Fact]
        public async Task RegisterAsync_WhenValid_ShouldCreateUserAndReturnTrue()
        {
            var request = RegisterRequest();

            _userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(request.Email))
                .ReturnsAsync((User)null);

            _userMappingMock
                .Setup(m => m.AuthRegisterRequestToEntity(request))
                .Returns(ValidUser());

            _userRepositoryMock
                .Setup(r => r.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(new User());

            var result = await _authService.RegisterAsync(request);

            Assert.True(result);
            _userRepositoryMock.Verify(
                r => r.CreateUserAsync(It.IsAny<User>()),
                Times.Once);
        }
    }
}
