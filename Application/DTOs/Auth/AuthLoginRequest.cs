using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.Auth
{
    public class AuthLoginRequest
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [PasswordPropertyText]
        public string Password { get; set; }
    }
}
