using MoviesApi.DTOs.User;
using MoviesApi.DTOs.User.Jwt;
using MoviesApi.Entities;

namespace MoviesApi.Mapping
{
    public class UserMapping
    {
        public User ToEntity(CreateUserRequest dto)
        {
            return new User
            {
                Name = dto.Name,
                Email = dto.Email,
                PasswordHash = dto.PasswordHash,
            };
        }

        public UserResponse ToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
                Role = user.Role,
                Status = user.Status,
                CreatedAt = user.CreatedAt,
                UpdatedAt = user.UpdatedAt,
                DeletedAt = user.DeletedAt
            };
        }

        public UserJwt ToJwtEntity(User user)
        {
            return new UserJwt
            {
                Id = user.Id,
                Email = user.Email,
                Status = user.Status,
                Role = user.Role
            };
        }
    }
}
