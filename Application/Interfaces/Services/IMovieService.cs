using Domain.DTOs.Movie;
using Domain.DTOs.Pagination;
using Domain.Entities;
using FluentResults;

namespace Domain.Interfaces.Services
{
    public interface IMovieService
    {
        Task<Result<MovieDetailsResponse>> CreateMovieAsync(CreateMovieRequest createMovieRequest);
        Task<Result<PaginationResponse<MovieTitleResponse>>> GetAllMovieAsync(
            PaginationParams paginationParams,
            string? title,
            string? genre,
            string? directors,
            string? cast);
        Task<Result<MovieDetailsResponse>> GetMovieByIdAsync(int id);
        Task<Result<IEnumerable<MovieTitleResponse>>> GetAllMoviesVotedByUser(int userId);
        Task<Result> UpdateMovieAsync(int id, UpdateMovie updateMovie);
        Task<Result> DeleteMovieAsync(int id);
    }
}
