using Domain.DTOs.Pagination;
using Domain.DTOs.User;
using Domain.Entities;
using Domain.Enums.User;
using Domain.Interfaces.Mappers;
using Domain.Interfaces.Repositories;
using Domain.Services;
using FluentAssertions;
using NSubstitute;

public class UserServiceTests
{
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUserMapping _mapping = Substitute.For<IUserMapping>();

    private readonly UserService _service;

    public UserServiceTests()
    {
        _service = new UserService(_userRepository, _mapping);
    }

    [Fact]
    public async Task CreateUserAsync_ShouldFail_WhenEmailAlreadyExists()
    {
        var request = new CreateUserRequest
        {
            Email = "novo@email.com",
            Password = "123456",
            Name = "Davi"
        };

        _userRepository.GetUserByEmailAsync(request.Email)
            .Returns(new User
            {
                Email = "novo2@email.com",
                Password = "12345656",
                Name = "Daniel"
            });

        var result = await _service.CreateUserAsync(request);

        result.IsFailed.Should().BeTrue();
        await _userRepository.DidNotReceive().CreateUserAsync(Arg.Any<User>());
    }

    [Fact]
    public async Task CreateUserAsync_ShouldCreateUser_WhenEmailNotExists()
    {
        var request = new CreateUserRequest
        {
            Email = "novo@email.com",
            Password = "123456",
            Name = "Davi"
        };

        _userRepository.GetUserByEmailAsync(request.Email)
            .Returns(null as User);

        var userEntity = new User
        {
            Email = request.Email,
            Password = "123456",
            Name = "Davi"
        };

        _mapping.CreateUserRequestToEntity(Arg.Any<CreateUserRequest>())
            .Returns(userEntity);

        _mapping.ToResponse(userEntity)
            .Returns(new UserResponse { Id = 1, Email = request.Email, Name = request.Name });

        var result = await _service.CreateUserAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Email.Should().Be(request.Email);

        await _userRepository.Received(1).CreateUserAsync(userEntity);
    }

    [Fact]
    public async Task GetAllUsersAsync_ShouldReturnPaginatedUsers()
    {
        var paginationParams = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var users = new List<User>
        {
            new User {
                Email = "novo@email.com",
                Password = "123456",
                Name = "Davi" },

            new User 
            {
                Email = "novo2@email.com",
                Password = "12345656",
                Name = "Daniel" 
            }
        };

        var pagedResult = new PaginationResponse<User>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalItems = 2,
            Items = users
        };

        _userRepository.GetAllUsersAsync(paginationParams)
            .Returns(pagedResult);

        _mapping.ToResponse(Arg.Any<User>())
            .Returns(x =>
            {
                var user = x.Arg<User>();
                return new UserResponse { Id = user.Id, Name = user.Name, Email = user.Email! };
            });

        var result = await _service.GetAllUsersAsync(paginationParams);

        result.IsSuccess.Should().BeTrue();
        result.Value.Items.Count().Should().Be(2);
        result.Value.TotalItems.Should().Be(2);
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldFail_WhenUserNotFound()
    {
        _userRepository.GetUserByIdAsync(1)
            .Returns(null as User);

        var result = await _service.GetUserByIdAsync(1);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task GetUserByIdAsync_ShouldReturnUser_WhenExists()
    {
        var user = new User
        {
            Email = "novo2@email.com",
            Password = "12345656",
            Name = "Daniel"
        };

        _userRepository.GetUserByIdAsync(1)
            .Returns(user);

        _mapping.ToResponse(user)
            .Returns(new UserResponse { Id = user.Id, Name = user.Name, Email = user.Email });

        var result = await _service.GetUserByIdAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Name.Should().Be("Daniel");
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldFail_WhenUserNotFound()
    {
        _userRepository.GetUserByIdAsync(1)
            .Returns(null as User);

        var result = await _service.UpdateUserAsync(1, new UpdateUser { Name = "Davi" });

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateUserAsync_ShouldUpdateUser_WhenExists()
    {
        var user = new User
        {
            Email = "novo2@email.com",
            Password = "12345656",
            Name = "Daniel"
        };

        _userRepository.GetUserByIdAsync(1)
            .Returns(user);

        var updateRequest = new UpdateUser { Name = "NewName" };

        var result = await _service.UpdateUserAsync(1, updateRequest);

        result.IsSuccess.Should().BeTrue();
        user.Name.Should().Be("NewName");

        await _userRepository.Received(1).UpdateUserAsync(user);
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldFail_WhenUserNotFound()
    {
        _userRepository.GetUserByIdAsync(1)
            .Returns(null as User);

        var result = await _service.DeleteUserAsync(1);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteUserAsync_ShouldSetInactive_WhenUserExists()
    {
        var user = new User
        {
            Email = "novo2@email.com",
            Password = "12345656",
            Name = "Daniel"
        };

        _userRepository.GetUserByIdAsync(1)
            .Returns(user);

        var result = await _service.DeleteUserAsync(1);

        result.IsSuccess.Should().BeTrue();
        user.Status.Should().Be(UserStatus.Inactive);

        await _userRepository.Received(1).DeleteUserAsync(user);
    }
}