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

        //[Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateUserAsync(CreateUserRequest createUserRequest, CancellationToken cancellationToken)
        {
            var user = await _userService.CreateUserAsync(createUserRequest);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult> GetUserByIdAsync(int id, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByIdAsync(id);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllUsersAsync(
            [FromQuery] PaginationParams paginationParams
            , CancellationToken cancellationToken
            )
        {
            var users = await _userService.GetAllUsersAsync(paginationParams);
            return Ok(users);
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        [Route("email")]
        public async Task<IActionResult> GetUserByEmailAsync(string email, CancellationToken cancellationToken)
        {
            var user = await _userService.GetUserByEmailAsync(email);
            return Ok(user);
        }

        [Authorize(Roles = "Admin")]
        [HttpPut]
        [Route("{id}")]
        public async Task<IActionResult> UpdateUserAsync(int id, UpdateUser updateUser, CancellationToken cancellationToken)
        {
            await _userService.UpdateUserAsync(id, updateUser);
            return Ok();
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteUserAsync(int id, CancellationToken cancellationToken)
        {
            await _userService.DeleteUserAsync(id);
            return Ok(); //alterar depois
        }
    }
}
