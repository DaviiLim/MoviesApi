using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Domain.DTOs.Auth
{
    public class AuthRegisterRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public string Password { get; set; }

        [PasswordPropertyText]
        [Required(ErrorMessage = "Confirm Password is required")]
        [StringLength(255, ErrorMessage = "Must be between 5 and 255 characters", MinimumLength = 5)]
        public string ConfirmPassword { get; set; }
    }
}
