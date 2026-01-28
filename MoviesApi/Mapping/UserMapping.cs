using MoviesApi.DTOs.User;
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
                ConfirmPassword = dto.ConfirmPassword
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
    }
}
