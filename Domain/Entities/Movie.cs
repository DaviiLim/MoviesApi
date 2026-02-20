using Domain.Enums.Movie;

namespace Domain.Entities
{
    public class Movie
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required string Synops { get; set; }
        public required string Classification { get; set; }
        public required string Genres { get; set; }
        public float Duration { get; set; }
        public required List<string> Cast { get; set; }
        public required List<string> Directors { get; set; }
        public MovieStatus Status { get; set; } = MovieStatus.Online;

        public int ReleasedYear { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public DateTime? DeletedAt { get; set; }

        public ICollection<Vote>? Votes { get; set; } 
    }
}
