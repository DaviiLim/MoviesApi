using System.Runtime.CompilerServices;

namespace MoviesApi.DTOs.Movie
{
    public class MovieTitleResponse
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public float AvarageScore { get; set; }
        public float TotalVotes { get; set; }
    }
}