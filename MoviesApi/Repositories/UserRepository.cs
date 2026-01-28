using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs.User;
using MoviesApi.Entities;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Mapping;
using System.Reflection.Metadata.Ecma335;

namespace MoviesApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly UserMapping _mapping;

        public UserRepository(AppDbContext context, UserMapping mapping)
        {
            _context = context;
            _mapping = mapping;
        }
        public async Task<UserResponse> CreateUserAsync(CreateUserRequest dto)
        {
            var entity = _mapping.ToEntity(dto);
            _context.AddAsync(entity);
            await _context.SaveChangesAsync();

            return _mapping.ToResponse(entity);
        }
        public async Task<UserResponse> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var response = users.Select(users => _mapping.ToResponse(users));

            return response;
        }
        public async Task<UserResponse?> GetUserByIdAsync(int id)
        {
            var response = await _context.Users.FindAsync(id);

            return response;
        }
        public async Task<UserResponse> UpdateUserAsync(int id, UpdateUser dto)
        {
            throw new NotImplementedException();
        }
    }
}
