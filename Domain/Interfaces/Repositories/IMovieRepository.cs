using Domain.Entities;

namespace Domain.Interfaces.Repositories
{
    public interface IMovieRepository
    {
        Task CreateMovieAsync(Movie movie);
        Task<IEnumerable<Movie>> GetAllMovieAsync();
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<Movie?> GetMovieByTitleAsync(string title);
        Task<IEnumerable<Movie>> GetAllMoviesVotedByUser(int userId);
        void UpdateMovieAsync(Movie movie);
        void DeleteMovieAsync(Movie movie);
    }
}
