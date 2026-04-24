using Application.DTOs.Movie;

namespace Application.DTOs.Vote
{
    public class VoteResponse
    {
        public int Id { get; set; }
        public required MovieDetailsResponse Movie { get; set; }
        public float Score { get; set; }
    }
}
