using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Repositories
{
    public interface IMovieRepository
    {
        Task<Movie> CreateMovieAsync(Movie movie);
        Task<IEnumerable<Movie>> GetAllMovieAsync();
        Task<Movie> GetMovieByIdAsync(int id);
        Task<bool> UpdateMovieAsync(Movie movie);
        Task<bool> DeleteMovieAsync(Movie movie);
    }
}
