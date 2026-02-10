using MoviesApi.DTOs.Vote;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Services
{
    public interface IVoteService
    {
        Task<bool> VoteAsync(int userId, CreateVoteRequest request);
        Task<bool> DeleteVoteAsync(int userId, int id);
    }
}
