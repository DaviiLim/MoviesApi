using Domain.Interfaces.Repositories;
using FluentResults;
using Domain.Errors;
using Application.DTOs.Auth;
using Application.DTOs.User;
using Application.Interfaces.Mappers;
using Application.Interfaces.Services;

namespace Application.Services
{
    public class AuthService : IAuthService
    {
        private readonly IJwtTokenService _tokenService;
        private readonly IUserRepository _userRepository;
        private readonly IUserMapping _mapping;

        public AuthService(IJwtTokenService tokenService, IUserRepository userRepository, IUserMapping mapping)
        {
            _tokenService = tokenService;
            _userRepository = userRepository;
            _mapping = mapping;
        }

        public async Task<Result<string>> LoginAsync(AuthLoginRequest authLoginRequest, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByEmailAsync(authLoginRequest.Email, cancellationToken);
            if (user == null)
                return Result.Fail(new NotFoundError("User not found."));

            if (!BCrypt.Net.BCrypt.Verify(authLoginRequest.Password, user.Password))
                return Result.Fail(new UnauthorizedError("Invalid credentials."));

            var userJwt = _mapping.ToJwtEntity(user);
            var token = _tokenService.GenerateToken(userJwt);

            return Result.Ok(token);
        }

        public async Task<Result<UserResponse>> RegisterAsync(AuthRegisterRequest authRegisterRequest, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByEmailAsync(authRegisterRequest.Email, cancellationToken);

            if (user != null)
                return Result.Fail(new ConflictError("Email already exists"));

            if (authRegisterRequest.Password != authRegisterRequest.ConfirmPassword)
                return Result.Fail(new UnauthorizedError("Invalid credentials."));

            string password = BCrypt.Net.BCrypt.HashPassword(authRegisterRequest.Password);

            authRegisterRequest.Password = password;

            var entityUser = await _userRepository.CreateUserAsync(_mapping.AuthRegisterRequestToEntity(authRegisterRequest), cancellationToken);
            var userResponse = _mapping.ToResponse(entityUser);

            return Result.Ok(userResponse);
        }
    }
}
