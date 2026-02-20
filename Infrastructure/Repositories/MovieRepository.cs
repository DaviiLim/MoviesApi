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

        public async Task CreateMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
        }

        public async Task<PaginationResponse<Movie>> GetAllMovieAsync(
            PaginationParams paginationParams,
            string? title,
            string? genre,
            string? directors,
            string? cast)
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

            var totalItems = await query.CountAsync();

            query = query
                .OrderByDescending(m => m.Votes!.Count())
                .ThenBy(m => m.Title);

            var pagedMovies = await query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToListAsync();

            return new PaginationResponse<Movie>
            {
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize,
                TotalItems = totalItems,
                Items = pagedMovies
            };
        }

        public async Task<Movie?> GetMovieByIdAsync(int id)
        {
            return await _context.Movies
                .Include(m => m.Votes)
                .FirstOrDefaultAsync(m => m.Id == id);
        }

        public async Task<Movie?> GetMovieByTitleAsync(string title)
        {
            return await _context.Movies.FirstOrDefaultAsync(m => m.Title == title);
        }


        public async Task<IEnumerable<Movie>> GetAllMoviesVotedByUser(int userId)
        {
            return await _context.Movies
                .Where(m => m.Votes!.Any(v => v.UserId == userId))
                .Include(m => m.Votes!.Where(v => v.UserId == userId))
                .ToListAsync();
        }


        public async Task UpdateMovieAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

        public async Task DeleteMovieAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
        }

    }
}
