using Domain.DTOs.Pagination;
using Domain.DTOs.User;
using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest createUserRequest);
        Task<PaginationResponse<UserResponse>> GetAllUsersAsync(PaginationParams paginationParams);
        Task<UserResponse> GetUserByIdAsync(int id);
        Task<UserResponse> GetUserByEmailAsync(string email);
        Task<bool> UpdateUserAsync(int id, UpdateUser dto);
        Task<bool> DeleteUserAsync(int id);
    }
}
