using MoviesApi.DTOs.User;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<UserResponse> CreateUserAsync(CreateUserRequest dto);
        Task<IEnumerable<UserResponse>> GetAllUsersAsync();
        Task<UserResponse> GetUserByIdAsync(int id);
        Task<UserResponse> UpdateUserAsync(int id, UpdateUser dto);
        Task<UserResponse> DeleteUserAsync(int id);
        Task<User> GetUserByEmail(string email);
    }
}
