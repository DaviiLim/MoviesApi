using Domain.Interfaces.Repositories;
using Microsoft.EntityFrameworkCore;
using Domain.Entities;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class VoteRepository : IVoteRepository
    {
        private readonly AppDbContext _context;

        public VoteRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task AddAsync(Vote vote, CancellationToken cancellationToken = default)
        {
            await _context.Votes.AddAsync(vote, cancellationToken);
        }

        public async Task SaveChangesAsync(CancellationToken cancellationToken = default)
        {
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<IEnumerable<Vote>> GetAllVotesAsync(CancellationToken cancellationToken = default)
        {
            return await _context.Votes.ToListAsync(cancellationToken);
        }

        public async Task<Vote?> GetVoteByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Votes.FirstOrDefaultAsync(v => v.Id == id, cancellationToken);
        }

        public async Task<Vote?> FindVoteAsync(int userId, int movieId, CancellationToken cancellationToken = default)
        {
            return await _context.Votes
                .IgnoreQueryFilters()
                .FirstOrDefaultAsync(v =>
                    v.UserId == userId &&
                    v.MovieId == movieId, cancellationToken);
        }

        public async Task DeleteVoteAsync(Vote vote, CancellationToken cancellationToken = default)
        {
            _context.Votes.Update(vote);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
