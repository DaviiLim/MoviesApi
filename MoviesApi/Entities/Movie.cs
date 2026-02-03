using MoviesApi.Enums.Movie;

namespace MoviesApi.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public string Synops { get; set; }
        public string Classification { get; set; }
        public string Genres { get; set; }
        public float Duration { get; set; }
        public List<string> Cast { get; set; }
        public List<string> Directors { get; set; }
        public MovieStatus Status { get; set; }
        public DateTime ReleasedYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }
    }
}
