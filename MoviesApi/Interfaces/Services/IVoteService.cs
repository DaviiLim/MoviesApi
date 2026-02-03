using MoviesApi.DTOs.Vote;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Services
{
    public interface IVoteService
    {
        Task<VoteResponse> VoteAsync(int userId, CreateVoteRequest createVoteRequest);
        Task<IEnumerable<VoteResponse>> GetAllVotesAsync();
        Task<VoteResponse?> GetVoteByIdAsync(int id);
        Task<bool> DeleteVoteAsync(int id);
    }
}
