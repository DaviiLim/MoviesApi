using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs.Vote;
using MoviesApi.Entities;
using MoviesApi.Interfaces.Mappers;

namespace MoviesApi.Mapping
{
    public class VoteMapping : IVoteMapping
    {

        public Vote CreateVoteRequestToEntity(CreateVoteRequest createVoteRequest)
        {
            return new Vote
            {
                MovieId = createVoteRequest.MovieId,
                Score = createVoteRequest.Score
            };
        }

        public VoteResponse ToResponse(Vote vote, MovieDetailsResponse movieResponse)
        {
            return new VoteResponse
            {
                Id = vote.Id,
                Movie = movieResponse,
                Score = vote.Score
            };
        }
    }
}
