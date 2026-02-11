using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Domain.DTOs.Vote;
using Domain.Interfaces.Services;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Domain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public VoteController(IVoteService voteService, IHttpContextAccessor httpContextAccessor)
        {
            _voteService = voteService;
            _httpContextAccessor = httpContextAccessor;
        }

        [Authorize(Roles = "DefaultUser, Admin")]
        [HttpPost]
        public async Task<IActionResult> VoteAsync(CreateVoteRequest createVoteRequest)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _voteService.VoteAsync(userId, createVoteRequest));
        }


        [Authorize(Roles = "DefaultUser, Admin")]
        [HttpDelete]
        [Route("{movieId}")]
        public async Task<IActionResult> DeleteVoteAsync(int movieId)
        {
            var userId = int.Parse(_httpContextAccessor.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier).Value);
            return Ok(await _voteService.DeleteVoteAsync(userId, movieId));
        }

    }
}
