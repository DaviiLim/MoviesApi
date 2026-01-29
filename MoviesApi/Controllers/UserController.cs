using Microsoft.AspNetCore.Mvc;
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
        
        [HttpGet]
        public async Task<IActionResult> Register()
        {
            return Ok(await _userService.GetAllUsersAsync());
        }
    }
}
