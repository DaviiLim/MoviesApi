using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Services
{
    public interface IMovieService
    {
        Task<MovieResponse> CreateMovieAsync(CreateMovieRequest createMovieRequest);
        Task<IEnumerable<MovieResponse>> GetAllMovieAsync();
        Task<MovieResponse> GetMovieByIdAsync(int id);
        Task<bool> UpdateMovieAsync(int id, UpdateMovie updateMovie);
        Task<bool> DeleteMovieAsync(int id);
    }
}
