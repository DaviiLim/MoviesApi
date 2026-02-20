using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs.User;
using Domain.Interfaces.Services;
using Domain.DTOs.Pagination;

namespace Domain.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            var user = await _userService.CreateUserAsync(createUserRequest);
            return Ok();
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllUsersAsync(paginationParams);
            return Ok(users);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UpdateUser updateUser, CancellationToken cancellationToken)
        {
            _userService.UpdateUserAsync(id, updateUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            _userService.DeleteUserAsync(id);
            return NoContent(); 
        }
    }
}
