using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IMovieRepository
    {
        Task<Movie> CreateMovieAsync(Movie movie);
        Task<IEnumerable<Movie>> GetAllMovieAsync();
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<Movie?> GetMovieByTitleAsync(string title);
        Task<IEnumerable<Movie>> GetAllMoviesVotedByUser(int userId);
        Task<bool> UpdateMovieAsync(Movie movie);
        Task<bool> DeleteMovieAsync(Movie movie);
    }
}
