using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Application.DTOs.Auth
{
    public class AuthRegisterRequest
    {
        [Required]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public required string Password { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public required string ConfirmPassword { get; set; }
    }
}
