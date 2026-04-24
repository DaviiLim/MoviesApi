using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs.Pagination;
using Application.DTOs.User;
using Application.Interfaces.Services;

namespace Api.Controllers
{
    [ApiController]
    [Authorize(Roles = "Admin")]
    [Route("api/[controller]")]
    public class UserController : BaseApiController
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
            return HandleResult(user);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return HandleResult(user);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync([FromQuery] PaginationParams paginationParams, CancellationToken cancellationToken)
        {
            var users = await _userService.GetAllUsersAsync(paginationParams);
            return HandleResult(users);
        }

        [HttpGet("email")]
        public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return HandleResult(user);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UpdateUser updateUser, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(id, updateUser);
            return NoContent();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(id);
            return NoContent();
        }
    }
}
