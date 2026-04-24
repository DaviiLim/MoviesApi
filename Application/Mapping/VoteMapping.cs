using Application.DTOs.Movie;
using Application.DTOs.Vote;
using Application.Interfaces.Mappers;
using Domain.Entities;

namespace Application.Mapping
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
