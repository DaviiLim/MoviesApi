using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Movie
{
    public class UpdateMovie
    {
        [Required(ErrorMessage = "Title is required")]
        [StringLength(255, MinimumLength = 5, ErrorMessage = "Title must be between 5 and 255 characters")]
        public string Title { get; set; }

        [Required(ErrorMessage = "Synopsis is required")]
        [StringLength(2000, MinimumLength = 10, ErrorMessage = "Synopsis must be between 10 and 2000 characters")]
        public string Synops { get; set; }

        [Required(ErrorMessage = "Classification is required")]
        [RegularExpression(@"^(G|PG|PG-13|R|NC-17|L|10|12|14|16|18)$",
            ErrorMessage = "Invalid classification")]
        public string Classification { get; set; }

        [Required(ErrorMessage = "Genres are required")]
        [StringLength(255, ErrorMessage = "Genres must not exceed 255 characters")]
        public string Genres { get; set; }

        [Required(ErrorMessage = "Duration is required")]
        [Range(1, 1000, ErrorMessage = "Duration must be between 1 and 1000 minutes")]
        public float Duration { get; set; }

        [Required(ErrorMessage = "Cast is required")]
        [MinLength(1, ErrorMessage = "At least one cast member is required")]
        public List<string> Cast { get; set; }

        [Required(ErrorMessage = "Directors are required")]
        [MinLength(1, ErrorMessage = "At least one director is required")]
        public List<string> Directors { get; set; }

        [Required(ErrorMessage = "Release year is required")]
        [Range(1888, 2100, ErrorMessage = "Release year must be a valid year")]
        public int ReleasedYear { get; set; }
    }
}
