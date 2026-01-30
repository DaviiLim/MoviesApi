using MoviesApi.DTOs.Auth;
using MoviesApi.DTOs.User;
using MoviesApi.DTOs.User.Jwt;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Mappers
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
