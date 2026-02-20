using Domain.Interfaces.Repositories;
using Domain.DTOs.Auth;
using Domain.Interfaces.Mappers;
using Domain.Interfaces.Services;
using FluentResults;
using Domain.Errors;

namespace Domain.Services
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

        public async Task<Result<string>> LoginAsync(AuthLoginRequest authLoginRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(authLoginRequest.Email);
            if (user == null)
                return Result.Fail(new NotFoundError("User not found."));

            if (!BCrypt.Net.BCrypt.Verify(authLoginRequest.Password, user.Password))
                return Result.Fail(new UnauthorizedError("Invalid credentials."));

            var userJwt = _mapping.ToJwtEntity(user);
            var token = _tokenService.GenerateToken(userJwt);

            return Result.Ok(token);
        }

        public async Task<Result> RegisterAsync(AuthRegisterRequest authRegisterRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(authRegisterRequest.Email);

            if (user != null)
                return Result.Fail(new ConflictError("Email already exists"));

            if (authRegisterRequest.Password != authRegisterRequest.ConfirmPassword)
                return Result.Fail(new UnauthorizedError("Invalid credentials."));

            string password = BCrypt.Net.BCrypt.HashPassword(authRegisterRequest.Password);

            authRegisterRequest.Password = password;
            
            await _userRepository.CreateUserAsync(_mapping.AuthRegisterRequestToEntity(authRegisterRequest));

            return Result.Ok();
        }
    }
}
