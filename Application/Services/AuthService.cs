using MoviesApi.DTOs.Auth;
using MoviesApi.Exceptions;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;

namespace MoviesApi.Services
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

        public async Task<string> LoginAsync(AuthLoginRequest authLoginRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(authLoginRequest.Email);
            if (user == null) throw new EmailNotFoundException();

            if (!BCrypt.Net.BCrypt.Verify(authLoginRequest.Password,user.Password))
                throw new InvalidCredentialsException();

            var userJwt = _mapping.ToJwtEntity(user);
            var token = _tokenService.GenerateToken(userJwt);
            return token;
        }

        public async Task<bool> RegisterAsync(AuthRegisterRequest authRegisterRequest)
        {
            var user = await _userRepository.GetUserByEmailAsync(authRegisterRequest.Email);

            if (user != null) throw new EmailAlreadyExistsException(); 

            if (authRegisterRequest.Password != authRegisterRequest.ConfirmPassword)
                throw new InvalidCredentialsException();

            string password = BCrypt.Net.BCrypt.HashPassword(authRegisterRequest.Password);

            authRegisterRequest.Password = password;
            
            await _userRepository.CreateUserAsync(_mapping.AuthRegisterRequestToEntity(authRegisterRequest));

            return true;
        }
    }
}
