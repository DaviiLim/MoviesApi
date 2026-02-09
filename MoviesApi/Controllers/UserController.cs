using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.Pagination;
using MoviesApi.DTOs.User;
using MoviesApi.Interfaces.Services;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            return Ok(await _userService.CreateUserAsync(createUserRequest));
        }

        [Authorize(Roles = "DefaultUser,Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id)
        {
            return Ok(await _userService.GetUserByIdAsync(id));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync(
            [FromQuery] PaginationParams paginationParams
            )
        {
            return Ok(await _userService.GetAllUsersAsync(paginationParams));
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("email/")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            return Ok(await _userService.GetUserByEmailAsync(email));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("update/{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UpdateUser updateUser)
        {
            return Ok(await _userService.UpdateUserAsync(id, updateUser));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("delete/{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            return Ok(await _userService.DeleteUserAsync(id)); //alterar depois
        }
    }
}
