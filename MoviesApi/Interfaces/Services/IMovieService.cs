using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs.Pagination;
using MoviesApi.Entities;

namespace MoviesApi.Interfaces.Services
{
    public interface IMovieService
    {
        Task<MovieDetailsResponse> CreateMovieAsync(CreateMovieRequest createMovieRequest);
        Task<PaginationResponse<MovieTitleResponse>> GetAllMovieAsync(PaginationParams paginationParams,string? title, string? genre, string? directors, string? cast);
        Task<MovieDetailsResponse> GetMovieByIdAsync(int id);
        Task<bool> UpdateMovieAsync(int id, UpdateMovie updateMovie);
        Task<bool> DeleteMovieAsync(int id);
    }
}
