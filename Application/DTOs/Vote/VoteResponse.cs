using Domain.DTOs.Movie;

namespace Domain.DTOs.Vote
{
    public class VoteResponse
    {
        public int Id { get; set; }
        public MovieDetailsResponse Movie { get; set; }
        public float Score { get; set; }
    }
}
