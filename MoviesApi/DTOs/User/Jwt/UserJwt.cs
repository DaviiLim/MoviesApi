using MoviesApi.Enums;

namespace MoviesApi.DTOs.User.Jwt
{
    public class UserJwt
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public UserStatus Status { get; set; }
        public UserRole Role { get; set; }
    }
}
