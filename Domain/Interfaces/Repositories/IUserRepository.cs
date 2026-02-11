using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id); 
        Task<bool> UpdateUserAsync(User user);
        Task<bool> DeleteUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
