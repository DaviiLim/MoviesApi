using Application.DTOs.User;
using Application.Interfaces.Mappers;
using Application.Interfaces.Services;
using Domain.DTOs.Pagination;
using Domain.Enums.User;
using Domain.Errors;
using Domain.Interfaces.Repositories;
using FluentResults;

namespace Application.Services
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

        public async Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken = default)
        {
            var userEmail = await _userRepository.GetUserByEmailAsync(createUserRequest.Email, cancellationToken);

            if (userEmail != null)
                return Result.Fail(new ConflictError("Email already exists."));

            string password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password);

            createUserRequest.Password = password;

            var user = _mapping.CreateUserRequestToEntity(createUserRequest);

            await _userRepository.CreateUserAsync(user, cancellationToken);

            var userResponse = _mapping.ToResponse(user);

            return Result.Ok(userResponse);
        }

        public async Task<Result<PaginationResponse<UserResponse>>> GetAllUsersAsync(PaginationParams paginationParams, CancellationToken cancellationToken = default)
        {
            var movies = await _userRepository
                .GetAllUsersAsync(paginationParams, cancellationToken);

            var response = new PaginationResponse<UserResponse>
            {
                PageNumber = movies.PageNumber,
                PageSize = movies.PageSize,
                TotalItems = movies.TotalItems,
                Items = movies.Items.Select(u => _mapping.ToResponse(u)).ToList()
            };

            return Result.Ok(response);
        }

        public async Task<Result<UserResponse>> GetUserByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);
            if (user == null)
                return Result.Fail(new NotFoundError("User not Found"));

            var userResponse = _mapping.ToResponse(user);

            return Result.Ok(userResponse);
        }

        public async Task<Result<UserResponse>> GetUserByEmailAsync(string email, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByEmailAsync(email, cancellationToken);
            if (user == null)
                return Result.Fail(new NotFoundError("User not Found"));

            var userResponse = _mapping.ToResponse(user);

            return Result.Ok(userResponse);
        }

        public async Task<Result> UpdateUserAsync(int id, UpdateUser updateUser, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
                return Result.Fail(new NotFoundError("User not Found"));

            user.Name = updateUser.Name;

            await _userRepository.UpdateUserAsync(user, cancellationToken);

            return Result.Ok();
        }

        public async Task<Result> DeleteUserAsync(int id, CancellationToken cancellationToken = default)
        {
            var user = await _userRepository.GetUserByIdAsync(id, cancellationToken);

            if (user == null)
                return Result.Fail(new NotFoundError("User not Found"));

            user.Status = UserStatus.Inactive;

            await _userRepository.DeleteUserAsync(user, cancellationToken);

            return Result.Ok();
        }
    }
}
