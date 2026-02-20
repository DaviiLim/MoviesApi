using Domain.Entities;
using Domain.DTOs.Pagination;

namespace Domain.Interfaces.Repositories
{
    public interface IMovieRepository
    {
        Task CreateMovieAsync(Movie movie);
        Task<PaginationResponse<Movie>> GetAllMovieAsync(PaginationParams paginationParams, string? title, string? genre, string? directors, string? cast);
        Task<Movie?> GetMovieByIdAsync(int id);
        Task<Movie?> GetMovieByTitleAsync(string title);
        Task<IEnumerable<Movie>> GetAllMoviesVotedByUser(int userId);
        Task UpdateMovieAsync(Movie movie);
        Task DeleteMovieAsync(Movie movie);
    }
}
