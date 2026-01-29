using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.Auth;
using MoviesApi.DTOs.User;
using MoviesApi.Interfaces.Services;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly IUserService _userService;

        public AuthController(IAuthService authService, IUserService userService)
        {
            _authService = authService;
            _userService = userService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
        {
            var token = await _authService.LoginAsync(request);
            return Ok(token);
        }
        //Mudar para um AuthRegister DTO
        [HttpPost("register")]
        public async Task<IActionResult> Register(CreateUserRequest dto)
        {
            var userResponse = await _userService.CreateUserAsync(dto);
            return  Ok(userResponse);
        }
    }
}
