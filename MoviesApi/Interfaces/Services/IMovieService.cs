using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Services
{
    public interface IMovieService
    {
        Task<MovieDetailsResponse> CreateMovieAsync(CreateMovieRequest createMovieRequest);
         Task<IEnumerable<MovieTitleResponse>> GetAllMovieAsync();
        Task<MovieDetailsResponse> GetMovieByIdAsync(int id);
        Task<bool> UpdateMovieAsync(int id, UpdateMovie updateMovie);
        Task<bool> DeleteMovieAsync(int id);
    }
}
