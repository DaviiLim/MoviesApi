using MoviesApi.DTOs.Pagination;
using MoviesApi.DTOs.User;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Services
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
