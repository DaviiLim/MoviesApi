using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs.Auth;
using Domain.Interfaces.Services;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : BaseApiController
    {
        private readonly IAuthService _authService;

        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] AuthLoginRequest request, CancellationToken cancellationToken)
        {
            var token = await _authService.LoginAsync(request);
            return HandleResult(token);
        }

        [AllowAnonymous]
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] AuthRegisterRequest authRegisterRequest, CancellationToken cancellationToken)
        {
            var user = await _authService.RegisterAsync(authRegisterRequest);

            return HandleResult(user);
        }
    }
}
