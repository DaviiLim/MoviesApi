using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs.Vote;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Mappers
{
    public interface IVoteMapping
    {
        public Vote CreateVoteRequestToEntity(CreateVoteRequest createVoteRequest);
        public VoteResponse ToResponse(Vote vote, MovieDetailsResponse movieResponse);
    }
}
