using MoviesApi.DTOs.Auth;

namespace MoviesApi.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<string> LoginAsync(AuthLoginRequest authLoginRequest);
        public Task<bool> RegisterAsync(AuthRegisterRequest authRegisterRequest);
    }
}
