using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<IEnumerable<User>> GetAllUsersAsync();
        Task<User?> GetUserByIdAsync(int id); 
        void UpdateUserAsync(User user);
        void DeleteUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
