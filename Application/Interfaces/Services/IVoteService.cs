using Domain.DTOs.Vote;
using Domain.Entities;
using FluentResults;

namespace Domain.Interfaces.Services
{
    public interface IVoteService
    {
        Task<Result> VoteAsync(int userId, CreateVoteRequest request);
        Task<Result> DeleteVoteAsync(int userId, int movieId);
    }
}
