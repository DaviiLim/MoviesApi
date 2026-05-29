using Domain.DTOs.Pagination;
using Domain.Entities;
using Domain.Interfaces.Repositories;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task CreateMovieAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            await _context.Movies.AddAsync(movie, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task<PaginationResponse<Movie>> GetAllMovieAsync(
            PaginationParams paginationParams,
            string? title,
            string? genre,
            string? directors,
            string? cast,
            CancellationToken cancellationToken = default)
        {
            var query = _context.Movies
                .Include(m => m.Votes)
                .AsNoTracking()
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(m =>
                    m.Title.ToLower().Contains(title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(m =>
                    m.Genres.ToLower().Contains(genre.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(directors))
            {
                query = query.Where(m =>
                    m.Directors.Any(d =>
                        d.ToLower().Contains(directors.ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(cast))
            {
                query = query.Where(m =>
                    m.Cast.Any(a =>
                        a.ToLower().Contains(cast.ToLower())));
            }

            var totalItems = await query.CountAsync(cancellationToken);

            query = query
                .OrderByDescending(m => m.Votes!.Count())
                .ThenBy(m => m.Title);

            var pagedMovies = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync(cancellationToken);

            return new PaginationResponse<Movie>
            {
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize,
                TotalItems = totalItems,
                Items = pagedMovies
            };
        }

        public async Task<Movie?> GetMovieByIdAsync(int id, CancellationToken cancellationToken = default)
        {
            return await _context.Movies
                .Include(m => m.Votes)
                .FirstOrDefaultAsync(m => m.Id == id, cancellationToken);
        }

        public async Task<Movie?> GetMovieByTitleAsync(string title, CancellationToken cancellationToken = default)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.Title == title, cancellationToken);
        }

        public async Task<IEnumerable<Movie>> GetAllMoviesVotedByUser(int userId, CancellationToken cancellationToken = default)
        {
            return await _context.Movies
                .Where(m => m.Votes!.Any(v => v.UserId == userId))
                .Include(m => m.Votes!.Where(v => v.UserId == userId))
                .ToListAsync(cancellationToken);
        }

        public async Task UpdateMovieAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync(cancellationToken);
        }

        public async Task DeleteMovieAsync(Movie movie, CancellationToken cancellationToken = default)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync(cancellationToken);
        }
    }
}
