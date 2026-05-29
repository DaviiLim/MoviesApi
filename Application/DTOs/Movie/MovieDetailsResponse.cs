namespace Application.DTOs.Movie
{
    public class MovieDetailsResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Synops { get; set; }
        public required string Classification { get; set; }
        public required string Genres { get; set; }
        public float Duration { get; set; }
        public float AverageScore { get; set; }
        public int TotalVotes { get; set; }
        public required List<string> Cast { get; set; }
        public required List<string> Directors { get; set; }
        public int ReleasedYear { get; set; }
    }
}
