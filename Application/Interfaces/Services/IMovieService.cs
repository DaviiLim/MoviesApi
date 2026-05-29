using Application.DTOs.Movie;
using Domain.DTOs.Pagination;
using FluentResults;

namespace Application.Interfaces.Services
{
    public interface IMovieService
    {
        Task<Result<MovieDetailsResponse>> CreateMovieAsync(CreateMovieRequest createMovieRequest, CancellationToken cancellationToken = default);
        Task<Result<PaginationResponse<MovieTitleResponse>>> GetAllMovieAsync(
            PaginationParams paginationParams,
            string? title,
            string? genre,
            string? directors,
            string? cast,
            CancellationToken cancellationToken = default);
        Task<Result<MovieDetailsResponse>> GetMovieByIdAsync(int id, CancellationToken cancellationToken = default);
        Task<Result<IEnumerable<MovieTitleResponse>>> GetAllMoviesVotedByUser(int userId, CancellationToken cancellationToken = default);
        Task<Result> UpdateMovieAsync(int id, UpdateMovie updateMovie, CancellationToken cancellationToken = default);
        Task<Result> DeleteMovieAsync(int id, CancellationToken cancellationToken = default);
    }
}
