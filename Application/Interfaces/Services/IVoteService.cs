using Application.DTOs.Vote;
using FluentResults;

namespace Application.Interfaces.Services
{
    public interface IVoteService
    {
        Task<Result> VoteAsync(int userId, CreateVoteRequest request, CancellationToken cancellationToken = default);
        Task<Result> DeleteVoteAsync(int userId, int movieId, CancellationToken cancellationToken = default);
    }
}
