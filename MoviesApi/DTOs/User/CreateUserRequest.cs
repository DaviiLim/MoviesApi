using MoviesApi.Enums.User;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.User
{
    public class CreateUserRequest
    {
        [Required]
        public string Name { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        public UserRole Role { get; set; }

        public string Password { get; set; }                                // reavalair

    }
}

