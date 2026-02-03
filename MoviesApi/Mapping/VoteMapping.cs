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
                UserId = createVoteRequest.UserId,
                MovieId = createVoteRequest.MovieId,
                Score = createVoteRequest.Score

            };
        }

        public VoteResponse ToResponse(Vote vote)
        {
            return new VoteResponse
            {
                Id = vote.Id,
                UserId = vote.UserId,
                MovieId = vote.MovieId,
                Score = vote.Score
            };

        }
    }
}
