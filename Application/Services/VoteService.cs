using Domain.Interfaces.Repositories;
using Domain.DTOs.Vote;
using Domain.Entities;
using Domain.Enums.Vote;
using Domain.Exceptions;
using Domain.Interfaces.Mappers;
using Domain.Interfaces.Services;

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

        public async Task<bool> VoteAsync(int userId, CreateVoteRequest request)
        {
            var user = await _userRepository.GetUserByIdAsync(userId)
                ?? throw new UserNotFoundException();

            var movie = await _movieRepository.GetMovieByIdAsync(request.MovieId)
                ?? throw new MovieNotFoundException();

            var existingVote = await _voteRepository
                .ExistsVoteAsync(userId, movie.Id);

            if (existingVote != null)
            {
                existingVote.Score = request.Score;
                existingVote.Status = VoteStatus.Active;
            }
            else
            {
                var vote = _mapping.CreateVoteRequestToEntity(request);
                vote.UserId = userId;
                vote.Status = VoteStatus.Active;

                await _voteRepository.AddAsync(vote);
            }

            await _voteRepository.SaveChangesAsync();
            return true;
        }

        public async Task<bool> DeleteVoteAsync(int userId,int movieId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new UserNotFoundException();

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);
            if (movie == null) throw new MovieNotFoundException();

            var votes = await _voteRepository.GetAllVotesAsync();

            var userVotedMovie = votes.FirstOrDefault(v => v.MovieId == movieId && v.UserId == userId);

            if (userVotedMovie == null) throw new UserHasNotVotedForMovieException();

            userVotedMovie.Status = VoteStatus.Inactive;
            userVotedMovie.DeletedAt = DateTime.Now;

            await _voteRepository.DeleteVoteAsync(userVotedMovie);

            return true;


        }
    }
}
