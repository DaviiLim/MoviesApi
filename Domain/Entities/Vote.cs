using Domain.Enums.Vote;

namespace Domain.Entities
{
    public class Vote
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public float Score { get; set; }
        public VoteStatus Status { get; set; } = VoteStatus.Active;
        public DateTime CreatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
        public User User { get; set; } = null!;
        public Movie Movie { get; set; } = null!;
    }
}
