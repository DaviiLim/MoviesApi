using Microsoft.EntityFrameworkCore;
using MoviesApi.Entities;
using MoviesApi.Interfaces.Repositories;
using System.Reflection.Metadata.Ecma335;

namespace MoviesApi.Repositories
{
    public class MovieRepository : IMovieRepository
    {
        private readonly AppDbContext _context;

        public MovieRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Movie> CreateMovieAsync(Movie movie)
        {
            await _context.Movies.AddAsync(movie);
            await _context.SaveChangesAsync();
            return movie;
        }

        public async Task<IEnumerable<Movie>> GetAllMovieAsync()
        {
            return await _context.Movies
                .Include(m => m.Votes)
                .ToListAsync();
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

        public async Task<bool> UpdateMovieAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteMovieAsync(Movie movie)
        {
            _context.Movies.Update(movie);
            await _context.SaveChangesAsync();
            return true;
        }

    }
}
