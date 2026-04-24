using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Application.DTOs.Auth
{
    public class AuthLoginRequest
    {
        [Required]
        [EmailAddress]
        public required string Email { get; set; }

        [Required]
        [PasswordPropertyText]
        public required string Password { get; set; }
    }
}
