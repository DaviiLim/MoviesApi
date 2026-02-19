
using Domain.DTOs.Movie;
using Domain.DTOs.Pagination;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Extensions;
using System.Security.Claims;

namespace Domain.Controllers
{

    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public MovieController(IMovieService movieService, IHttpContextAccessor httpContextAccessor)
        {
            _movieService = movieService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMovieAsync(CreateMovieRequest createMovieRequest, CancellationToken cancellationToken)
        {
            return Ok(await _movieService.CreateMovieAsync(createMovieRequest));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMovieByIdAsync(int id, CancellationToken cancellationToken)
        {
            return Ok(await _movieService.GetMovieByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovieAsync(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? title, string? genre, string? directors, string? cast,
            CancellationToken cancellationToken
            )

        {
            return Ok(await _movieService.GetAllMovieAsync(paginationParams,title, genre, directors, cast));
        }

        [Authorize]
        [HttpGet]
        [Route("voted/")]
        public async Task<IActionResult> GetUserVotedMovies(CancellationToken cancellationToken)
        {
            var userId = User.GetUserIdFromToken();
            return Ok(await _movieService.GetAllUserMovies(userId));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateMovieAsync(int id, UpdateMovie updateMovie, CancellationToken cancellationToken)
        {
            _movieService.UpdateMovieAsync(id, updateMovie);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id, CancellationToken cancellationToken)
        {
            _movieService.DeleteMovieAsync(id);
            return Ok();
        }

    }

}
