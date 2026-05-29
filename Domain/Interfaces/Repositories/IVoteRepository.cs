using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IVoteRepository
    {
        Task SaveChangesAsync(CancellationToken cancellationToken = default);
        Task AddAsync(Vote vote, CancellationToken cancellationToken = default);
        Task<IEnumerable<Vote>> GetAllVotesAsync(CancellationToken cancellationToken = default);
        Task<Vote?> GetVoteByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Vote?> FindVoteAsync(int userId, int movieId, CancellationToken cancellationToken = default);
        Task DeleteVoteAsync(Vote vote, CancellationToken cancellationToken = default);
    }
}
