using Azure.Core;
using Domain.DTOs.Vote;
using Domain.Entities;
using Domain.Enums.Vote;
using Domain.Errors;
using Domain.Interfaces.Mappers;
using Domain.Interfaces.Repositories;
using Domain.Interfaces.Services;
using FluentResults;

namespace Domain.Services
{
    public class VoteService : IVoteService
    {
        private readonly IVoteRepository _voteRepository;
        private readonly IUserRepository _userRepository;
        private readonly IMovieRepository _movieRepository;
        private readonly IVoteMapping _mapping;
        public VoteService(IVoteMapping mapping, IVoteRepository voteRepository, IUserRepository userRepository, IMovieRepository movieRepository)
        {
            _voteRepository = voteRepository;
            _userRepository = userRepository;
            _movieRepository = movieRepository;
            _mapping = mapping;
        }

        public async Task<Result> VoteAsync(int userId, CreateVoteRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
                if (user == null)
                return Result.Fail(new NotFoundError("User not found."));

            var movie = await _movieRepository.GetMovieByIdAsync(request.MovieId);
            if (movie == null)
                return Result.Fail(new NotFoundError("Movie not found."));

            var existingVote = await _voteRepository
                .ExistsVoteAsync(userId, movie.Id);

            if (existingVote == null)
            {
                var vote = _mapping.CreateVoteRequestToEntity(request);
                vote.UserId = userId;

                await _voteRepository.AddAsync(vote);
                await _voteRepository.SaveChangesAsync();
                return Result.Ok();
            }

            existingVote.Score = request.Score;
            existingVote.Status = VoteStatus.Active;

            await _voteRepository.SaveChangesAsync();
            return Result.Ok();
        }

        public async Task<Result> DeleteVoteAsync(int userId,int movieId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                return Result.Fail(new NotFoundError("User not found."));

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);
            if (movie == null)
                return Result.Fail(new NotFoundError("Movie not found."));

            var votes = await _voteRepository.GetAllVotesAsync();

            var userVotedMovie = votes.FirstOrDefault(v => v.MovieId == movieId && v.UserId == userId);

            if (userVotedMovie == null)
                return Result.Fail(new NotFoundError("Vote not Found."));

            userVotedMovie.Status = VoteStatus.Inactive;
            userVotedMovie.DeletedAt = DateTime.Now;

            _voteRepository.DeleteVoteAsync(userVotedMovie);
            return Result.Ok();
        }
    }
}
