using Application.DTOs.Auth;
using Application.DTOs.User;
using Domain.DTOs.User.Jwt;
using Domain.Entities;

namespace Application.Interfaces.Mappers
{
    public interface IUserMapping
    {
        public User CreateUserRequestToEntity(CreateUserRequest createUserRequest);
        public User AuthRegisterRequestToEntity(AuthRegisterRequest authRegisterRequest);
        public User UpdateUserToEntity(UpdateUser updateUser);
        public UserResponse ToResponse(User user);
        public UserJwt ToJwtEntity(User user);
    }
}
