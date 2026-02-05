using MoviesApi.DTOs.User;
using MoviesApi.Enums.User;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;

namespace MoviesApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserMapping _mapping;

        public UserService(IUserRepository userRepository, IUserMapping mapping)
        {
            _userRepository = userRepository;
            _mapping = mapping;
        }

        //    ---------- testes
        public async Task<UserResponse> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            var userEmail = await _userRepository.GetUserByEmailAsync(createUserRequest.Email);

            if (userEmail != null) throw new Exception("Email already exist.");

            string password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password);

            createUserRequest.Password = password;

            var user = await _userRepository.CreateUserAsync(_mapping.CreateUserRequestToEntity(createUserRequest));

            return _mapping.ToResponse(user);
        }

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var usersResponse = users.Select(users => _mapping.ToResponse(users));
            return usersResponse;
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var userResponse = await _userRepository.GetUserByIdAsync(id);
            if (userResponse == null) throw new BadHttpRequestException("User not Found");
            return _mapping.ToResponse(userResponse);
        }


        public async Task<UserResponse> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) throw new BadHttpRequestException("User not Found");

            return _mapping.ToResponse(user);
        }

        //Reavaliar
        public async Task<bool> UpdateUserAsync(int id, UpdateUser updateUser)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) throw new BadHttpRequestException("User not Found");

            user.Name = updateUser.Name;

            await _userRepository.UpdateUserAsync(user);

            return true;
        }

        //Fazer
        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) throw new BadHttpRequestException("User not Found");

            user.Status = UserStatus.Inativo;

            await _userRepository.UpdateUserAsync(user);

            return true;
        }
    }
}
