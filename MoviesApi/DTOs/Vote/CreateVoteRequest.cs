namespace MoviesApi.DTOs.Vote
{
    public class CreateVoteRequest
    {
        public int UserId { get; set; }
        public int MovieId { get; set; }
        public float Score { get; set; }
    }
}
