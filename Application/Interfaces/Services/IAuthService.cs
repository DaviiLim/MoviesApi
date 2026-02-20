using Domain.DTOs.Auth;
using FluentResults;

namespace Domain.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<Result<string>> LoginAsync(AuthLoginRequest authLoginRequest);
        public Task<Result> RegisterAsync(AuthRegisterRequest authRegisterRequest);
    }
}
