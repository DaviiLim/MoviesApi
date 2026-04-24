using Application.DTOs.Movie;
using Application.DTOs.Vote;
using Domain.Entities;

namespace Application.Interfaces.Mappers
{
    public interface IVoteMapping
    {
        public Vote CreateVoteRequestToEntity(CreateVoteRequest createVoteRequest);
        public VoteResponse ToResponse(Vote vote, MovieDetailsResponse movieResponse);
    }
}
