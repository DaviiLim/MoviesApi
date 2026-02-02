using Microsoft.AspNetCore.Authorization;
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

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequest request)
        {
            var token = await _authService.LoginAsync(request);
            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody]AuthRegisterRequest authRegisterRequest)
        {
            await _authService.RegisterAsync(authRegisterRequest);
            
            return  Ok();
        }
    }
}
