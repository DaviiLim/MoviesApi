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

        //    ---------- Para adms criarem usuários
        public async Task<Result<UserResponse>> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            var userEmail = await _userRepository.GetUserByEmailAsync(createUserRequest.Email);

            if (userEmail != null)
                return Result.Fail(new ConflictError("Email already exists."));

            string password = BCrypt.Net.BCrypt.HashPassword(createUserRequest.Password);

            createUserRequest.Password = password;

            var user = _mapping.CreateUserRequestToEntity(createUserRequest);

            await _userRepository.CreateUserAsync(user);

            var userResponse = _mapping.ToResponse(user);

            return Result.Ok(userResponse);
        }

        public async Task<Result<PaginationResponse<UserResponse>>> GetAllUsersAsync(PaginationParams paginationParams)
        {
            var movies = await _userRepository
                .GetAllUsersAsync(paginationParams);

            var response = new PaginationResponse<UserResponse>
            {
                PageNumber = movies.PageNumber,
                PageSize = movies.PageSize,
                TotalItems = movies.TotalItems,
                Items = movies.Items.Select(u => _mapping.ToResponse(u)).ToList()
            };

            return Result.Ok(response);
        }

        public async Task<Result<UserResponse>> GetUserByIdAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);
            if (user == null)
                return Result.Fail(new NotFoundError("User not Found"));

            var userResponse = _mapping.ToResponse(user);

            return Result.Ok(userResponse);
        }


        public async Task<Result<UserResponse>> GetUserByEmailAsync(string email)
        {
            var user = await _userRepository.GetUserByEmailAsync(email);
            if (user == null)
                return Result.Fail(new NotFoundError("User not Found"));

            var userResponse = _mapping.ToResponse(user);

            return Result.Ok(userResponse);
        }

        public async Task<Result> UpdateUserAsync(int id, UpdateUser updateUser)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return Result.Fail(new NotFoundError("User not Found"));

            user.Name = updateUser.Name;

            await _userRepository.UpdateUserAsync(user);

            return Result.Ok();
        }

        public async Task<Result> DeleteUserAsync(int id)
        {
            var user = await _userRepository.GetUserByIdAsync(id);

            if (user == null)
                return Result.Fail(new NotFoundError("User not Found"));

            user.Status = UserStatus.Inactive;

            await _userRepository.DeleteUserAsync(user);

            return Result.Ok();
        }
    }
}
