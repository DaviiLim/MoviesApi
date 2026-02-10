using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;
using MoviesApi.Interfaces.Repositories;

namespace MoviesApi.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly AppDbContext _context;

        public VoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Vote vote)
        {
            await _context.Votes.AddAsync(vote);
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IEnumerable<Vote>> GetAllVotesAsync()
        {
            return await _context.Votes.ToListAsync();
        }

        public async Task<Vote?> GetVoteByIdAsync(int id)
        {
            return await _context.Votes.FindAsync(id); ;
        }

        public async Task<Vote?> ExistsVoteAsync(int userId, int movieId)
        {
            return await _context.Votes
                .IgnoreQueryFilters() 
                .FirstOrDefaultAsync(v =>
                    v.UserId == userId &&
                    v.MovieId == movieId);
        }

        public async Task<bool> DeleteVoteAsync(Vote vote)
        {
            _context.Votes.Update(vote);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
