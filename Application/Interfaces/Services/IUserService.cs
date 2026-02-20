using Domain.DTOs.Pagination;
using Domain.DTOs.User;
using Domain.Entities;
using FluentResults;

namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest createUserRequest);
        Task<Result<PaginationResponse<UserResponse>>> GetAllUsersAsync(PaginationParams paginationParams);
        Task<Result<UserResponse>> GetUserByIdAsync(int id);
        Task<Result<UserResponse>> GetUserByEmailAsync(string email);
        Task<Result> UpdateUserAsync(int id, UpdateUser updateUser);
        Task<Result> DeleteUserAsync(int id);
    }
}
