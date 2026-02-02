using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Movie
{
    public class UpdateMovie
    {
        [Required]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string Title { get; set; }

        [Required]
        public string Synops { get; set; }

        [Required]
        public string Classification { get; set; }

        [Required]
        public string Genres { get; set; }

        [Required]
        public float Duration { get; set; }

        [Required]
        public List<string> Cast { get; set; }

        [Required]
        public List<string> Directors { get; set; }
    }
}
