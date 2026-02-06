
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs.Pagination;
using MoviesApi.DTOs.User;
using MoviesApi.Interfaces.Services;
using MoviesApi.Services;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MovieController : ControllerBase
    {
        private readonly IMovieService _movieService;

        public MovieController(IMovieService movieService)
        {
            _movieService = movieService;
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateMovieAsync(CreateMovieRequest createMovieRequest)
        {
            return Ok(await _movieService.CreateMovieAsync(createMovieRequest));
        }

        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
            return Ok(await _movieService.GetMovieByIdAsync(id));
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMovieAsync(
            [FromQuery] PaginationParams paginationParams,
            [FromQuery] string? title, string? genre, string? directors, string? cast)
        {

            return Ok(await _movieService.GetAllMovieAsync(paginationParams,title, genre, directors, cast));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateMovieAsync(int id, UpdateMovie updateMovie)
        {
            return Ok(await _movieService.UpdateMovieAsync(id, updateMovie));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteMovieAsync(int id)
        {
            return Ok(await _movieService.DeleteMovieAsync(id));
        }

    }

}
