using Domain.DTOs.Movie;
using Domain.DTOs.Vote;
using Domain.Entities;
using Domain.Interfaces.Mappers;

namespace Domain.Mapping
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
