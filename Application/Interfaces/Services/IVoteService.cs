using Application.DTOs.Vote;
using FluentResults;

namespace Application.Interfaces.Services
{
    public interface IVoteService
    {
        Task<Result> VoteAsync(int userId, CreateVoteRequest request);
        Task<Result> DeleteVoteAsync(int userId, int movieId);
    }
}
