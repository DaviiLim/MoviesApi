using Domain.DTOs.Auth;
using Domain.DTOs.User;
using Domain.DTOs.User.Jwt;
using Domain.Entities;
using Domain.Interfaces.Mappers;

namespace Domain.Mapping
{
    public class UserMapping : IUserMapping
    {
        public User CreateUserRequestToEntity(CreateUserRequest createUserRequest)
        {
            return new User
            {
                Name = createUserRequest.Name,
                Email = createUserRequest.Email,
                Password = createUserRequest.Password,
                Role = createUserRequest.Role
            };
        }

        public User AuthRegisterRequestToEntity(AuthRegisterRequest authRegisterRequest)
        {
            return new User
            {
                Name = authRegisterRequest.Name,
                Email = authRegisterRequest.Email,
                Password = authRegisterRequest.Password,
            };
        }

        public User UpdateUserToEntity(UpdateUser updateUser)
        {
            return new User
            {
                Name = updateUser.Name,
            };
        }

        public UserResponse ToResponse(User user)
        {
            return new UserResponse
            {
                Id = user.Id,
                Name = user.Name,
                Email = user.Email,
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
