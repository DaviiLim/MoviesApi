using Microsoft.EntityFrameworkCore;
using MoviesApi.DTOs.User;
using MoviesApi.DTOs.Vote;
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

        public async Task<Vote> VoteAsync(Vote vote)                      //void?
        {
            await _context.Votes.AddAsync(vote);
            await _context.SaveChangesAsync();

            return vote;
        }

        public async Task<IEnumerable<Vote>> GetAllMovieVotes(int movieId)
        {
            return await _context.Votes.ToListAsync();
        }


        public async Task<Vote?> GetVoteByIdAsync(int id)
        {
            return await _context.Votes.FindAsync(id); ;
        }

        public async Task<bool> DeleteVoteAsync(Vote vote)
        {
            _context.Votes.Update(vote);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
