using Domain.Enums.User;
using System.ComponentModel.DataAnnotations;

namespace Domain.DTOs.User.Jwt
{
    public class UserJwt
    {
        [Key]
        public int Id { get; set; }
        [Required]
        [EmailAddress]
        public required string Email { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
    }
}
