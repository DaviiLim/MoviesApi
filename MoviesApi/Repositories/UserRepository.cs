using Azure;
using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs.Auth;
using MoviesApi.DTOs.User;
using MoviesApi.Entities;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Mapping;
using System.Reflection.Metadata.Ecma335;

namespace MoviesApi.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly AppDbContext _context;
        private readonly IUserMapping _mapping;

        public UserRepository(AppDbContext context, UserMapping mapping)
        {
            _context = context;
            _mapping = mapping;
        }
        public async Task<User> CreateUserAsync(AuthRegisterRequest authRegisterRequest)
        {
            var user = _mapping.AuthRegisterRequestToEntity(authRegisterRequest);
            _context.Users.Add(user);
            await _context.SaveChangesAsync();
            return user;
        }

        public async Task<User> GetUserByEmailAsync(string email)
        {
            var user = await _context.Users.FirstOrDefaultAsync(u => u.Email == email);
            return user;
        }

        public async Task<User> GetUserByIdAsync(int id)
        {
            var user = (await _context.Users.FindAsync(id));
            return user;
        }
        public async Task<IEnumerable<User>> GetAllUsersAsync()
        {
            return await _context.Users.ToListAsync(); ;
        }

        public async Task<bool> UpdateUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

        //Fazer
        public async Task<bool> DeleteUserAsync(User user)
        {
            _context.Users.Update(user);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
