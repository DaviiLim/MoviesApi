
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.Movie;
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

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetMovieByIdAsync(int id)
        {
            return Ok(await _movieService.GetMovieByIdAsync(id));
        }

        [Authorize(Roles = "DefaultUser,Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllMovieAsync()
        {
            return Ok(await _movieService.GetAllMovieAsync());
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
