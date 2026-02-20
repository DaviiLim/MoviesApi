using Domain.DTOs.Vote;
using Domain.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.Extensions;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace Domain.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService, IHttpContextAccessor httpContextAccessor)
        {
            _voteService = voteService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> VoteAsync(CreateVoteRequest createVoteRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserIdFromToken();
            await _voteService.VoteAsync(userId, createVoteRequest);
            return NoContent();
        }


        [Authorize]
        [HttpDelete]
        [Route("{movieId}")]
        public async Task<IActionResult> DeleteVoteAsync(int movieId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserIdFromToken();
            await _voteService.DeleteVoteAsync(userId, movieId);
            return NoContent();
        }

    }
}
