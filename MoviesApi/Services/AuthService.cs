using MoviesApi.DTOs.Auth;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;
using MoviesApi.Mapping;

namespace MoviesApi.Services
{
    public class AuthService : IAuthService
    {
        private readonly IUserService _userService;
        private readonly IJwtTokenService _tokenService;
        private readonly IUserRepository _userRepository;                   //tirar depois de usar 
        private readonly UserMapping _mapping;                             //Faço uma interface? 

        public AuthService(IUserService userService, IJwtTokenService tokenService, IUserRepository userRepository, UserMapping mapping)
        {
            _userService = userService;
            _tokenService = tokenService;
            _userRepository = userRepository;
            _mapping = mapping;
        }

        public async Task<string> LoginAsync(AuthLoginRequest request)
        {
            var user = await _userRepository.GetUserByEmail(request.Email);

            // Colocar Validador Hash de Senha

            if(user == null)
            {
                throw new Exception("User not Found");
            }

            var userJwt = _mapping.ToJwtEntity(user);
            var token = _tokenService.GenerateToken(userJwt);
            return token;
        }
    }
}
