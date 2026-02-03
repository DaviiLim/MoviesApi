using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Repositories
{
    public interface IVoteRepository
    {
        Task<Vote> VoteAsync(Vote vote);
        Task<IEnumerable<Vote>> GetMovieScore(int movieId);
        Task<Vote?> GetVoteByIdAsync(int id);
        Task<bool> DeleteVoteAsync(Vote vote);
    }
}
