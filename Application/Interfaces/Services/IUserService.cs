using Application.DTOs.User;
using Domain.DTOs.Pagination;
using FluentResults;

namespace Application.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken = default);
        Task<Result<PaginationResponse<UserResponse>>> GetAllUsersAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<UserResponse>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
        Task<Result> UpdateUserAsync(int id, UpdateUser updateUser, CancellationToken cancellationToken = default);
        Task<Result> DeleteUserAsync(int id, CancellationToken cancellationToken = default);
    }
}
