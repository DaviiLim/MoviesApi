using Domain.DTOs.Vote;
using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IVoteService
    {
        void VoteAsync(int userId, CreateVoteRequest request);
        void DeleteVoteAsync(int userId, int id);
    }
}
