using Domain.DTOs.Movie;
using Domain.DTOs.Pagination;
using Domain.Entities;

namespace Domain.Interfaces.Services
{
    public interface IMovieService
    {
        Task<MovieDetailsResponse> CreateMovieAsync(CreateMovieRequest createMovieRequest);
        Task<PaginationResponse<MovieTitleResponse>> GetAllMovieAsync(PaginationParams paginationParams,string? title, string? genre, string? directors, string? cast);
        Task<MovieDetailsResponse> GetMovieByIdAsync(int id);
        Task<IEnumerable<MovieTitleResponse>> GetAllUserMovies(int userId);
        Task<bool> UpdateMovieAsync(int id, UpdateMovie updateMovie);
        Task<bool> DeleteMovieAsync(int id);
    }
}
