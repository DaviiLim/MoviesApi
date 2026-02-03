using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using MoviesApi.DTOs.Vote;
using MoviesApi.Interfaces.Services;
using System.Reflection.Metadata.Ecma335;
using System.Security.Claims;

namespace MoviesApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class VoteController : ControllerBase
    {
        private readonly IVoteService _voteService;

        public VoteController(IVoteService voteService)
        {
            _voteService = voteService;
        }

        [Authorize(Roles = "DefaultUser, Admin")]
        [HttpPost]
        public async Task<IActionResult> VoteAsync(CreateVoteRequest createVoteRequest)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
            await _voteService.VoteAsync(userId, createVoteRequest);
            return Ok(await _voteService.VoteAsync(userId, createVoteRequest));
        }

        [Authorize(Roles = "DefaultUser, Admin")]
        [HttpGet]
        public async Task<IActionResult> GetAllVotesAsync()
        {
           
            return Ok(await _voteService.GetAllVotesAsync());
        }

        [Authorize(Roles = "DefaultUser, Admin")]
        [HttpGet]
        [Route("{id}")]
        public async Task<IActionResult?> GetVoteByIdAsync(int id)
        {
            return Ok(await _voteService.GetVoteByIdAsync(id));
        }

        [Authorize(Roles = "DefaultUser, Admin")]
        [HttpDelete]
        [Route("{id}")]
        public async Task<IActionResult> DeleteVoteAsync(int id)
        {
            return Ok(await _voteService.DeleteVoteAsync(id));
        }
    }
}
