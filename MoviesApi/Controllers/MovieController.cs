
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs.Movie;
using Domain.DTOs.Pagination;
using Domain.DTOs.User;
using Domain.Interfaces.Services;
using Domain.Services;
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

        [Authorize(Roles = "DefaultUser, Admin")]
        [HttpGet]
        [Route("MyList/")]
        public async Task<IActionResult> GetAllUserMovies()
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _movieService.GetAllUserMovies(userId));
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
