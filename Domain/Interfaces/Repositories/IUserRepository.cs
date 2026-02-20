using Domain.DTOs.Pagination;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user);
        Task<PaginationResponse<User>> GetAllUsersAsync(PaginationParams paginationParams);
        Task<User?> GetUserByIdAsync(int id);
        Task UpdateUserAsync(User user);
        Task DeleteUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
