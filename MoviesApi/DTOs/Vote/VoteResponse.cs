using MoviesApi.DTOs.Movie;

namespace MoviesApi.DTOs.Vote
{
    public class VoteResponse
    {
        public int Id { get; set; }
        public MovieResponse Movie { get; set; }
        public float Score { get; set; }
    }
}
