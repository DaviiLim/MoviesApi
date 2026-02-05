using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs.Vote;
using MoviesApi.Entities;
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

        public VoteService(IVoteRepository voteRepository)
        {
            _voteRepository = voteRepository;
        }

        public async Task<bool> VoteAsync(int userId, CreateVoteRequest createVoteRequest)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not found");

            var movie = await _movieRepository.GetMovieByIdAsync(createVoteRequest.MovieId);
            if (movie == null) throw new Exception("Movie not found");

            var vote = _mapping.CreateVoteRequestToEntity(createVoteRequest);
            vote.UserId = userId;

            await _voteRepository.VoteAsync(vote);

            return true;
        }

        public async Task<float> GetMovieScore(int userId, int movieId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not Found");

            var movie = await _movieRepository.GetMovieByIdAsync(movieId);
            if (movie == null) throw new Exception("Movie not Found");

            var votes = await _voteRepository.GetAllVotesAsync();
            var movieVotes = votes.Where(v => v.MovieId == movieId).ToList();

            if (!movieVotes.Any()) return 0;

            return movieVotes.Average(v => v.Score);
        }

        public async Task<List<VoteResponse>> GetUserVotedMovies(int userId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null)
                throw new Exception("User not Found");

            var votes = await _voteRepository.GetAllVotesAsync();

            var userVotes = votes
                .Where(v => v.UserId == userId)
                .Select(v => new VoteResponse
                {
                    Id = v.Id,
                    Score = v.Score,
                    Movie = new MovieResponse
                    {
                        Id = v.Movie.Id,
                        Title = v.Movie.Title,
                        Synops = v.Movie.Synops,
                        Classification = v.Movie.Classification,
                        Genres = v.Movie.Genres,
                        Duration = v.Movie.Duration,
                        Cast = v.Movie.Cast,
                        Directors = v.Movie.Directors,
                        ReleasedYear = v.Movie.ReleasedYear
                    }
                })
                .ToList();

            return userVotes;
        }



        public async Task<bool> DeleteVoteAsync(int userId,int voteId)
        {
            var user = await _userRepository.GetUserByIdAsync(userId);
            if (user == null) throw new Exception("User not Found");

            var vote = await _voteRepository.GetVoteByIdAsync(voteId);

            if (vote.UserId != userId) throw new BadHttpRequestException("Bad Request");

            vote.DeletedAt = DateTime.Now;
            return true;


        }
    }
}
