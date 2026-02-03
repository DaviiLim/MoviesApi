namespace MoviesApi.DTOs.Vote
{
    public class VoteResponse
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public float Score { get; set; }

    }
}
