using Api.Extensions;
using Application.DTOs.Vote;
using Application.Interfaces.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : BaseApiController
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> VoteAsync(CreateVoteRequest createVoteRequest, CancellationToken cancellationToken)
        {
            var userId = User.GetUserIdFromToken();
            var result = await _voteService.VoteAsync(userId, createVoteRequest, cancellationToken);
            return HandleResult(result);
        }


        [Authorize]
        [HttpDelete]
        [Route("{movieId}")]
        public async Task<IActionResult> DeleteVoteAsync(int movieId, CancellationToken cancellationToken)
        {
            var userId = User.GetUserIdFromToken();
            var result = await _voteService.DeleteVoteAsync(userId, movieId, cancellationToken);
            return HandleResult(result);
        }

    }
}
