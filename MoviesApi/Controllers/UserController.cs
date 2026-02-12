using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs.Pagination;
using Domain.DTOs.User;
using Domain.Interfaces.Services;

namespace Domain.Controllers
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

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest createUserRequest)
        {
            return Ok(await _userService.CreateUserAsync(createUserRequest));
        }

        [Authorize(Roles = "Admin")]
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
        [Route("email")]
        public async Task<IActionResult> GetUserByEmailAsync(string email)
        {
            return Ok(await _userService.GetUserByEmailAsync(email));
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UpdateUser updateUser)
        {
            return Ok(await _userService.UpdateUserAsync(id, updateUser));
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id)
        {
            return Ok(await _userService.DeleteUserAsync(id)); //alterar depois
        }
    }
}
