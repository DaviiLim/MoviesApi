using Domain.DTOs.Auth;
using Domain.DTOs.User;
using FluentResults;

namespace Domain.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<Result<string>> LoginAsync(AuthLoginRequest authLoginRequest);
        public Task<Result<UserResponse>> RegisterAsync(AuthRegisterRequest authRegisterRequest);
    }
}
