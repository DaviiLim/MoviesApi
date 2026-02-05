using MoviesApi.DTOs.Vote;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Services
{
    public interface IVoteService
    {
        Task<bool> VoteAsync(int userId, CreateVoteRequest createVoteRequest);
        Task<List<VoteResponse>> GetUserVotedMovies(int userId);
        Task<float> GetMovieScore(int userId, int movieId);
        Task<bool> DeleteVoteAsync(int userId, int id);
    }
}
