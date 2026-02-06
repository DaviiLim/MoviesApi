using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.Vote
{
    public class CreateVoteRequest
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        [Range(0, 4, ErrorMessage = "Score must be between 0 and 4")]
        public float Score { get; set; }
    }
}
