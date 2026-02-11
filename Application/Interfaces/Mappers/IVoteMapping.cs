using Domain.DTOs.Movie;
using Domain.DTOs.Vote;
using Domain.Entities;

namespace Domain.Interfaces.Mappers
{
    public interface IVoteMapping
    {
        public Vote CreateVoteRequestToEntity(CreateVoteRequest createVoteRequest);
        public VoteResponse ToResponse(Vote vote, MovieDetailsResponse movieResponse);
    }
}
