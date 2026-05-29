using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Vote
{
    public class CreateVoteRequest
    {
        [Required]
        public int MovieId { get; set; }

        [Required]
        [Range(0, 10, ErrorMessage = "Score must be between 0 and 10")]
        public float Score { get; set; }
    }
}
