using MoviesApi.DTOs.User;
using MoviesApi.Entities;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;
using MoviesApi.Mapping;

namespace MoviesApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly UserMapping _mapping;

        public UserService(IUserRepository userRepository, UserMapping mapping)
        {
            _userRepository = userRepository;
            _mapping = mapping;
        }
        public async Task<UserResponse> CreateUserAsync(CreateUserRequest dto)
        {   
            
            var userResponse = await _userRepository.CreateUserAsync(dto);
            return userResponse;
        }

        public Task<UserResponse> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            return _userRepository.GetAllUsersAsync();
        }

        public Task<User> GetUserByEmail(string email)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> GetUserByIdAsync(int id)
        {
            throw new NotImplementedException();
        }

        public Task<UserResponse> UpdateUserAsync(int id, UpdateUser dto)
        {
            throw new NotImplementedException();
        }
    }
}
