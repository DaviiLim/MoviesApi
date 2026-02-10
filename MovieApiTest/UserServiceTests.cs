using Moq;
using MoviesApi.DTOs.User;
using MoviesApi.Entities;
using MoviesApi.Exceptions;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Mapping;
using MoviesApi.Services;

namespace MovieApiTest
{
    public class UserServiceTests
    {
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly UserMapping _userMapping;
        private readonly UserService _userService;

        public UserServiceTests()
        {
            _userRepositoryMock = new Mock<IUserRepository>();
            _userMapping = new UserMapping();

            _userService = new UserService(_userRepositoryMock.Object, _userMapping);
        }
        [Fact]
        public async Task CreateUserAsync_EmailAlreadyExists_ShouldThrowEmailAlreadyExistsException()
        {
            var createUserRequest = new CreateUserRequest
            {
                Name = "string",
                Email = "user@example.com",
                Password = "string",
                Role = 0
            };

            var user2 = new MoviesApi.Entities.User
            {
                Id = 1,
                Name = "userName2",
                Email = "userEmai2@example.com",
                Password = "passwordExample2",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            _userRepositoryMock.Setup(r => r.GetUserByEmailAsync(createUserRequest.Email)).ReturnsAsync(user2);
            await Assert.ThrowsAsync<EmailAlreadyExistsException>(() => _userService.CreateUserAsync(createUserRequest));

            _userRepositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task CreateUserAsync_WhenEmailDoesntExist_ShouldReturnUserResponse()
        {
            var createUserRequest = new CreateUserRequest
            {
                Name = "userName2",
                Email = "userEmai2@example.com",
                Password = "string",
                Role = 0
            };

            var user = new User
            {
                Id = 1,
                Name = "userName2",
                Email = "userEmai2@example.com",
                Password = "passwordExample2",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            _userRepositoryMock
                .Setup(r => r.GetUserByEmailAsync(createUserRequest.Email))
                .ReturnsAsync((User)null);

            _userRepositoryMock
                .Setup(r => r.CreateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(user);

            var result = await _userService.CreateUserAsync(createUserRequest);

            Assert.NotNull(result);
            Assert.IsType<UserResponse>(result);
            Assert.Equal(createUserRequest.Name, result.Name);
            Assert.Equal(createUserRequest.Email, result.Email);

            _userRepositoryMock.Verify(r => r.CreateUserAsync(It.IsAny<User>()), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserExists_ShouldReturnUser()
        {
            var user = new User
            {
                Id = 1,
                Name = "userNameUpdated",
                Email = "userEmai2@example.com",
                Password = "passwordExample1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(user.Id))
                .ReturnsAsync(user);

            var result = await _userService.GetUserByIdAsync(user.Id);
            Assert.IsType<UserResponse>(result);
            Assert.NotNull(result);

            _userRepositoryMock.Verify(r => r.GetUserByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task GetUserByIdAsync_WhenUserDoesntExists_ShouldReturnUserNotFoundException()
        {
            var user = new User
            {
                Id = 1,
                Name = "userNameUpdated",
                Email = "userEmai2@example.com",
                Password = "passwordExample1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            _userRepositoryMock.Setup(r =>
            r.GetUserByIdAsync(user.Id))
                .ReturnsAsync((User)null);

            await Assert
                .ThrowsAsync<UserNotFoundException>(() =>
                _userService.GetUserByIdAsync(user.Id));

            _userRepositoryMock
                .Verify(r =>
                r.GetUserByIdAsync(It.IsAny<int>()), Times.Once);
        }

        [Fact]
        public async Task UpdateUserAsync_WhenUserExists_ShouldReturnTrue()
        {
            var user = new User
            {
                Id = 1,
                Name = "userNameUpdated",
                Email = "userEmai2@example.com",
                Password = "passwordExample1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            var userUpdate = new UpdateUser
            {
                Name = "userNameUpdated"
            };

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(r => r.UpdateUserAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            var result = await _userService.UpdateUserAsync(user.Id, userUpdate);

            Assert.True(result);

            _userRepositoryMock.Verify(
                r => r.UpdateUserAsync(It.IsAny<User>()),
                Times.Once);
        }


        [Fact]
        public async Task UpdateUserAsync_WhenUserDoesntExists_ShouldReturnUserNotFoundException()
        {
            var user = new User
            {
                Id = 1,
                Name = "userNameUpdated",
                Email = "userEmai2@example.com",
                Password = "passwordExample1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            var userUpdate = new UpdateUser
            {
                Name = "userNameUpdated"
            };

            _userRepositoryMock.Setup(r => 
            r.GetUserByIdAsync(user.Id))
                .ReturnsAsync((User)null);

            await Assert
                .ThrowsAsync<UserNotFoundException>(() => 
                _userService.UpdateUserAsync(user.Id, userUpdate));

            _userRepositoryMock
                .Verify(r => 
                r.UpdateUserAsync(It.IsAny<User>()), Times.Never);
        }

        [Fact]
        public async Task DeleteUserAsync_WhenUserExists_ShouldReturnTrue()
        {
            var user = new MoviesApi.Entities.User
            {
                Id = 1,
                Name = "userNameUpdated",
                Email = "userEmai2@example.com",
                Password = "passwordExample1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(user.Id))
                .ReturnsAsync(user);

            _userRepositoryMock
                .Setup(r => r.DeleteUserAsync(It.IsAny<User>()))
                .ReturnsAsync(true);

            var result = await _userService.DeleteUserAsync(user.Id);

            Assert.True(result);

            _userRepositoryMock.Verify(
                r => r.DeleteUserAsync(It.IsAny<User>()),
                Times.Once);
        }


        [Fact]
        public async Task DeleteUserAsync_WhenUserDoesntExists_ShouldReturnUserNotFoundException()
        {
            var user = new MoviesApi.Entities.User
            {
                Id = 1,
                Name = "userNameUpdated",
                Email = "userEmai2@example.com",
                Password = "passwordExample1",
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = null,
                DeletedAt = null
            };

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(user.Id))
                .ReturnsAsync((User)null);

            await Assert
                .ThrowsAsync<UserNotFoundException>(() => 
                _userService.DeleteUserAsync(user.Id));

            _userRepositoryMock
                .Verify(r => 
                r.DeleteUserAsync(It.IsAny<User>()), Times.Never);
        }
    }
}
