using Domain.Interfaces.Repositories;
using Domain.DTOs.Movie;
using Domain.DTOs.Pagination;
using Domain.DTOs.User;
using Domain.Enums.User;
using Domain.Exceptions;
using Domain.Interfaces.Mappers;
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

            var user = await _userRepository.CreateUserAsync(_mapping.CreateUserRequestToEntity(createUserRequest));

            return _mapping.ToResponse(user);
        }

        public async Task<PaginationResponse<UserResponse>> GetAllUsersAsync(PaginationParams paginationParams)
        {
            var users = await _userRepository.GetAllUsersAsync();
            var query = users.AsQueryable();

            var totalItems = query.Count();

            query = query
                .OrderBy(u => u.Name);

            var pagedUsers = query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            var items = pagedUsers.Select(u => _mapping.ToResponse(u));

            return new PaginationResponse<UserResponse>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };
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

        public async Task<bool> UpdateUserAsync(int id, UpdateUser updateUser)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) throw new UserNotFoundException();

            user.Name = updateUser.Name;

            await _userRepository.UpdateUserAsync(user);

            return true;
        }

        public async Task<bool> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null) throw new UserNotFoundException();

            user.Status = UserStatus.Inactive;

            await _userRepository.DeleteUserAsync(user);

            return true;
        }
    }
}
