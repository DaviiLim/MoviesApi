using Domain.DTOs.Auth;

namespace Domain.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<string> LoginAsync(AuthLoginRequest authLoginRequest);
        public Task<bool> RegisterAsync(AuthRegisterRequest authRegisterRequest);
    }
}
