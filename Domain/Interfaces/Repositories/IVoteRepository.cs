using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Repositories
{
    public interface IVoteRepository
    {
        Task SaveChangesAsync();
        Task AddAsync(Vote vote);
        Task<IEnumerable<Vote>> GetAllVotesAsync();
        Task<Vote?> GetVoteByIdAsync(int id);
        Task<Vote?> ExistsVoteAsync(int userId, int movieId);
        Task<bool> DeleteVoteAsync(Vote vote);
    }
}
