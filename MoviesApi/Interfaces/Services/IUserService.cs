using MoviesApi.DTOs.User;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Services
{
    public interface IUserService
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest dto);
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<UserResponse> GetUserByIdAsync(int id);
        Task<UserResponse> UpdateUserAsync(int id, UpdateUser dto);
        Task<UserResponse> DeleteUserAsync(int id);
        Task<User> GetUserByEmail(string email);
    }
}
