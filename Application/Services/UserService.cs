using Domain.DTOs.Movie;
using Domain.DTOs.Pagination;
using Domain.DTOs.User;
using Domain.Enums.User;
using Domain.Exceptions;
using Domain.Interfaces.Mappers;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;

namespace Domain.Services
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

        //    ---------- Para adms criarem usuários
        public async Task<UserResponse> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            var userEmail = await _userRepository.GetUserByEmailAsync(createUserRequest.Email);

            if (userEmail != null) throw new EmailAlreadyExistsException();

            string password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password);

            createUserRequest.Password = password;

            var user = _mapping.CreateUserRequestToEntity(createUserRequest);

            await _userRepository.CreateUserAsync(user);

            return _mapping.ToResponse(user);
        }

        public async Task<PaginationResponse<UserResponse>> GetAllUsersAsync(PaginationParams paginationParams)
        {
            var movies = await _userRepository
                .GetAllUsersAsync(paginationParams);

            var response = new PaginationResponse<UserResponse>
            {
                PageNumber = movies.PageNumber,
                PageSize = movies.PageSize,
                TotalItems = movies.TotalItems,
                Items = movies.Items.Select( u => _mapping.ToResponse(u) ).ToList()
            };

            return response;
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var userResponse = await _userRepository.GetUserByIdAsync(id);
            if (userResponse == null) throw new UserNotFoundException();
            return _mapping.ToResponse(userResponse);
        }


        public async Task<UserResponse> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null) throw new UserNotFoundException();

            return _mapping.ToResponse(user);
        }

        public async void UpdateUserAsync(int id, UpdateUser updateUser)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) throw new UserNotFoundException();

            user.Name = updateUser.Name;

            _userRepository.UpdateUserAsync(user);
        }

        public async void DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) throw new UserNotFoundException();

            user.Status = UserStatus.Inactive;

            _userRepository.DeleteUserAsync(user);
        }
    }
}
