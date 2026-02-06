namespace MoviesApi.DTOs.Movie
{
    public class MovieDetailsResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synops { get; set; }
        public string Classification { get; set; }
        public string Genres { get; set; }
        public float Duration { get; set; }
        public float AvarageScore { get; set; }
        public float TotalVotes { get; set; }
        public List<string> Cast { get; set; }
        public List<string> Directors { get; set; }
        public int ReleasedYear { get; set; }
    }
}
