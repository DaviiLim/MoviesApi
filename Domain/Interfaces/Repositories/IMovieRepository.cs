using Domain.Entities;
using Domain.DTOs.Pagination;

namespace Domain.Interfaces.Repositories
{
    public interface IMovieRepository
    {
        Task CreateMovieAsync(Movie movie, CancellationToken cancellationToken = default);
        Task<PaginationResponse<Movie>> GetAllMovieAsync(PaginationParams paginationParams, string? title, string? genre, string? directors, string? cast, CancellationToken cancellationToken = default);
        Task<Movie?> GetMovieByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Movie?> GetMovieByTitleAsync(string title, CancellationToken cancellationToken = default);
        Task<IEnumerable<Movie>> GetAllMoviesVotedByUser(int userId, CancellationToken cancellationToken = default);
        Task UpdateMovieAsync(Movie movie, CancellationToken cancellationToken = default);
        Task DeleteMovieAsync(Movie movie, CancellationToken cancellationToken = default);
    }
}
