using System.Threading.Tasks;
using Application.DTOs.Auth;
using Application.Interfaces.Mappers;
using Application.Interfaces.Services;
using Application.Services;
using Domain.DTOs.User.Jwt;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class AuthServiceTests
{
    private readonly IJwtTokenService _tokenService = Substitute.For<IJwtTokenService>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IUserMapping _mapping = Substitute.For<IUserMapping>();

    private readonly AuthService _service;

    public AuthServiceTests()
    {
        _service = new AuthService(_tokenService, _userRepository, _mapping);
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenUserNotFound()
    {
        _userRepository.GetUserByEmailAsync("teste@email.com")
            .Returns(null as User);

        var request = new AuthLoginRequest
        {
            Email = "teste@email.com",
            Password = "123"
        };

        var result = await _service.LoginAsync(request);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task LoginAsync_ShouldFail_WhenPasswordIsInvalid()
    {
        var user = new User
        {
                    
            Name = "Name Test",
            Email = "teste@email.com",
            Password = BCrypt.Net.BCrypt.HashPassword("correctPassword")
        };

        _userRepository.GetUserByEmailAsync(user.Email)
            .Returns(user);

        var request = new AuthLoginRequest
        {
            Email = user.Email,
            Password = "wrongPassword"
        };

        var result = await _service.LoginAsync(request);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task LoginAsync_ShouldReturnToken_WhenCredentialsAreValid()
    {
        var user = new User
        {
            Id = 1,
            Name = "Name Test",
            Email = "teste@email.com",
            Password = BCrypt.Net.BCrypt.HashPassword("123456")
        };

        _userRepository.GetUserByEmailAsync(user.Email)
            .Returns(user);

        var jwtEntity = new UserJwt { Email = user.Email}; 

        _mapping.ToJwtEntity(user).Returns(jwtEntity);
        _tokenService.GenerateToken(jwtEntity).Returns("fake-jwt-token");

        var request = new AuthLoginRequest
        {
            Email = user.Email,
            Password = "123456"
        };

        var result = await _service.LoginAsync(request);

        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be("fake-jwt-token");
    }


    [Fact]
    public async Task RegisterAsync_ShouldFail_WhenEmailAlreadyExists()
    {
        _userRepository.GetUserByEmailAsync("teste@email.com")
            .Returns(new User
            {
                Name = "Name Test",
                Email = "teste@email.com",
                Password = "123"
            });

        var request = new AuthRegisterRequest
        {
            Name = "Name Test",
            Email = "teste@email.com",
            Password = "123",
            ConfirmPassword = "123"
        };

        var result = await _service.RegisterAsync(request);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task RegisterAsync_ShouldFail_WhenPasswordsDoNotMatch()
    {
        _userRepository.GetUserByEmailAsync("teste@email.com")
            .Returns(null as User);

        var request = new AuthRegisterRequest
        {
            Name = "Name Test",
            Email = "teste@email.com",
            Password = "123",
            ConfirmPassword = "456"
        };

        var result = await _service.RegisterAsync(request);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task RegisterAsync_ShouldCreateUser_WhenValid()
    {
        _userRepository.GetUserByEmailAsync("novo@email.com")
            .Returns(null as User);

        var request = new AuthRegisterRequest
        {
            Name = "Name Test",
            Email = "novo@email.com",
            Password = "123456",
            ConfirmPassword = "123456"
        };

        var userEntity = new User { Name = request.Name, Email = request.Email };

        _mapping.AuthRegisterRequestToEntity(Arg.Any<AuthRegisterRequest>())
            .Returns(userEntity);

        var result = await _service.RegisterAsync(request);

        result.IsSuccess.Should().BeTrue();
        await _userRepository.Received(1).CreateUserAsync(userEntity);
    }
}