
using Domain.DTOs.Movie;
using Domain.DTOs.Pagination;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Controllers;
using MoviesApi.Extensions;
using System.Security.Claims;

namespace Domain.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : BaseApiController
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService, IHttpContextAccessor httpContextAccessor)
        {
            _movieService = movieService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMovieAsync(CreateMovieRequest createMovieRequest, CancellationToken cancellationToken)
        {
            var movie = await _movieService.CreateMovieAsync(createMovieRequest);
            return HandleResult(movie);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMovieByIdAsync(int id, CancellationToken cancellationToken)
        {
            var movie = await _movieService.GetMovieByIdAsync(id);
            return HandleResult(movie);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovieAsync(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? title, string? genre, string? directors, string? cast,
            CancellationToken cancellationToken
            )
        {
            var movies = await _movieService.GetAllMovieAsync(paginationParams, title, genre, directors, cast);
            return HandleResult(movies);
        }

        [Authorize]
        [HttpGet("voted")]
        public async Task<IActionResult> GetUserVotedMovies(CancellationToken cancellationToken)
        {
            var userId = User.GetUserIdFromToken();
            var userVotedMovies = await _movieService.GetAllMoviesVotedByUser(userId);
            return HandleResult(userVotedMovies);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMovieAsync(int id, UpdateMovie updateMovie, CancellationToken cancellationToken)
        {
            await _movieService.UpdateMovieAsync(id, updateMovie);
            return NoContent();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id, CancellationToken cancellationToken)
        {
            await _movieService.DeleteMovieAsync(id);
            return NoContent();
        }

    }

}
