using MoviesApi.DTOs.Vote;
using MoviesApi.Exceptions;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;

namespace MoviesApi.Services
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

        public async Task<bool> VoteAsync(int userId, CreateVoteRequest createVoteRequest)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new UserNotFoundException();

            var movie = await _movieRepository.GetMovieByIdAsync(createVoteRequest.MovieId);
            if (movie == null) throw new MovieNotFoundException();

            var vote = _mapping.CreateVoteRequestToEntity(createVoteRequest);
            vote.UserId = userId;

            await _voteRepository.VoteAsync(vote);

            return true;
        }

        public async Task<bool> DeleteVoteAsync(int userId,int voteId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new UserNotFoundException();

            var vote = await _voteRepository.GetVoteByIdAsync(voteId);

            if (vote.UserId != userId) throw new ForbiddenUserVoteException();

            vote.DeletedAt = DateTime.Now;
            return true;


        }
    }
}
