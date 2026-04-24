using Application.DTOs.Auth;
using Application.DTOs.User;
using FluentResults;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        public Task<Result<string>> LoginAsync(AuthLoginRequest authLoginRequest);
        public Task<Result<UserResponse>> RegisterAsync(AuthRegisterRequest authRegisterRequest);
    }
}
