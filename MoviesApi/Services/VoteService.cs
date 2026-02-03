using MoviesApi.DTOs.Vote;
using MoviesApi.Entities;
using MoviesApi.Enums.User;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;
using MoviesApi.Repositories;

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

        public VoteService(IVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<VoteResponse> VoteAsync(int userId, CreateVoteRequest createVoteRequest)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var movie = await _movieRepository.GetMovieByIdAsync(createVoteRequest.MovieId);
            if (movie == null) throw new Exception("Movie not found");

            var vote = _mapping.CreateVoteRequestToEntity(createVoteRequest);
            vote.UserId = userId;

            await _voteRepository.VoteAsync(vote);

            vote.Movie = movie;
            vote.User = user;

            return _mapping.ToResponse(vote);
        }

        public async Task<VoteResponse?> GetVoteByIdAsync(int id)
        {
            var vote = await _voteRepository.GetVoteByIdAsync(id);
            if (vote == null) throw new BadHttpRequestException("User not Found");

            var user = await _userRepository.GetUserByIdAsync(vote.UserId);
            if (user == null) throw new Exception("User not found");

            var movie = await _movieRepository.GetMovieByIdAsync(vote.MovieId);
            if (movie == null) throw new Exception("Movie not found");

            vote.Movie = movie;
            vote.User = user;

            return _mapping.ToResponse(vote);
        }

        public async Task<bool> DeleteVoteAsync(int id)
        {
            var vote = await _voteRepository.GetVoteByIdAsync(id);

            if (vote == null) throw new BadHttpRequestException("User not Found");

            vote.DeletedAt = DateTime.Now;

            await _voteRepository.DeleteVoteAsync(vote);

            return true;


        }

        public Task<IEnumerable<VoteResponse>> GetAllVotesAsync()
        {
            throw new NotImplementedException();
        }


    }
}
