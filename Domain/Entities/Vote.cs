using MoviesApi.Enums.Vote;

namespace MoviesApi.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public float Score { get; set; }
        public VoteStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public User? User { get; set; }
        public Movie? Movie { get; set; }
    }
}
