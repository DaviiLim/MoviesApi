using Domain.DTOs.Pagination;
using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IUserRepository
    {
        Task<User> CreateUserAsync(User user, CancellationToken cancellationToken = default);
        Task<PaginationResponse<User>> GetAllUsersAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default);
        Task<User?> GetUserByIdAsync(int id, CancellationToken cancellationToken = default);
        Task UpdateUserAsync(User user, CancellationToken cancellationToken = default);
        Task DeleteUserAsync(User user, CancellationToken cancellationToken = default);
        Task<User?> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default);
    }
}
