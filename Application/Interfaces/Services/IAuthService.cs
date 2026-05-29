using Application.DTOs.Auth;
using Application.DTOs.User;
using FluentResults;

namespace Application.Interfaces.Services
{
    public interface IAuthService
    {
        Task<Result<string>> LoginAsync(AuthLoginRequest authLoginRequest, CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> RegisterAsync(AuthRegisterRequest authRegisterRequest, CancellationToken cancellationToken = default);
    }
}
