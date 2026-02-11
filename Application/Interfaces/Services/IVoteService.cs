using Domain.DTOs.Vote;
using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IVoteService
    {
        Task<bool> VoteAsync(int userId, CreateVoteRequest request);
        Task<bool> DeleteVoteAsync(int userId, int id);
    }
}
