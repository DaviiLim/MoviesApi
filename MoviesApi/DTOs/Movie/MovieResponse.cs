namespace MoviesApi.DTOs.Movie
{
    public class MovieResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synops { get; set; }
        public string Classification { get; set; }
        public string Genres { get; set; }
        public float Duration { get; set; }
        public List<string> Cast { get; set; }
        public List<string> Directors { get; set; }
    }
}
