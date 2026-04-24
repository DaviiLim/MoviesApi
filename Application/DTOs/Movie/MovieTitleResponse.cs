using System.Runtime.CompilerServices;

namespace Application.DTOs.Movie
{
    public class MovieTitleResponse
    {
        public int Id { get; set; }
        public required string Title { get; set; }
        public required List<string> Cast { get; set; }
        public required List<string> Directors { get; set; }
        public required string Genres { get; set; }
        public float AvarageScore { get; set; }
        public float TotalVotes { get; set; }
    }
}