using MoviesApi.DTOs.Auth;
using MoviesApi.DTOs.User;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(AuthRegisterRequest authRegisterRequest);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User> GetUserByIdAsync(int id); 
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<User> GetUserByEmailAsync(string email);
    }
}
