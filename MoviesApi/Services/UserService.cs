using Azure.Core;
using MoviesApi.DTOs.User;
using MoviesApi.Entities;
using MoviesApi.Enums;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;
using MoviesApi.Mapping;

namespace MoviesApi.Services
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUserMapping _mapping;

        public UserService(IUserRepository userRepository, UserMapping mapping)
        {
            _userRepository = userRepository;
            _mapping = mapping;
        }

        //public async Task<UserResponse> CreateUserAsync(CreateUserRequest dto)
        //{
        //     return;
        //}

        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _userRepository.GetAllUsersAsync();
            var usersResponse = users.Where(u => u.Status != UserStatus.Inativo).Select(users => _mapping.ToResponse(users));
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
        public async Task<UserResponse> UpdateUserAsync(int id, UpdateUser updateUser)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) throw new BadHttpRequestException("User not Found");

            user.Name = updateUser.Name;

            await _userRepository.UpdateUserAsync(user);

            return _mapping.ToResponse(user);
        }

        //Fazer
        public async Task<UserResponse> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) throw new BadHttpRequestException("User not Found");

            user.Status = UserStatus.Inativo;

            await _userRepository.UpdateUserAsync(user);

            return _mapping.ToResponse(user);
        }
    }
}
