using MoviesApi.Enums;
using System.ComponentModel.DataAnnotations;

namespace MoviesApi.DTOs.User.Jwt
{
    public class UserJwt
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
    }
}
