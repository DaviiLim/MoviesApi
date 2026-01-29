using Azure;
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
            var user = _mapping.ToEntity(dto);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return _mapping.ToResponse(user);
        }

        public async Task<User> GetUserByEmail(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<UserResponse> GetUserByIdAsync(int id)
        {
            var user = (await _context.Users.FindAsync(id));
            return _mapping.ToResponse(user);
        }
        //Fazer
        public async Task<UserResponse> DeleteUserAsync(int id)
        {
            throw new NotImplementedException();
        }
        public async Task<IEnumerable<UserResponse>> GetAllUsersAsync()
        {
            var users = await _context.Users.ToListAsync();
            var usersResponse = users.Select(users => _mapping.ToResponse(users));
            return usersResponse;
        }
        //Fazer
        public async Task<UserResponse> UpdateUserAsync(int id, UpdateUser dto)
        {
            throw new NotImplementedException();
        }
    }
}
