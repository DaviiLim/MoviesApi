using Domain.Enums.User;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.User
{
    public class CreateUserRequest
    {
        [Required]
        [StringLength(100, MinimumLength = 3)]
        public required string Name { get; set; }

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public required string Email { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 6)]
        public required string Password { get; set; }

        public UserRole Role { get; set; }

    }
}

