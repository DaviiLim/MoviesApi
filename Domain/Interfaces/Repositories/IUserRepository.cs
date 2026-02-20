using Domain.DTOs.Pagination;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task CreateUserAsync(User user);
        Task<PaginationResponse<User>> GetAllUsersAsync(PaginationParams paginationParams);
        Task<User?> GetUserByIdAsync(int id); 
        void UpdateUserAsync(User user);
        void DeleteUserAsync(User user);
        Task<User?> GetUserByEmailAsync(string email);
    }
}
