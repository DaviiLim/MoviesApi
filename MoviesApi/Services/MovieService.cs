using MoviesApi.DTOs.Movie;
using MoviesApi.Enums.Movie;
using MoviesApi.Exceptions;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;
using System.Data;

namespace MoviesApi.Services
{
    public class MovieService : IMovieService
    {
        private readonly IMovieRepository _movieRepository;
        private readonly IMovieMapping _mapping;

        public MovieService(IMovieRepository movieRepository, IMovieMapping movieMapping)
        {
            _movieRepository = movieRepository;
            _mapping = movieMapping;
        }

        public async Task<MovieDetailsResponse> CreateMovieAsync(CreateMovieRequest createMovieRequest)
        {
            var movie = _mapping.CreateMovieRequestToEntity(createMovieRequest);
            await _movieRepository.CreateMovieAsync(movie);
            return _mapping.ToDetailsResponse(movie, 0,0);
        }

        public async Task<IEnumerable<MovieTitleResponse>> GetAllMovieAsync()
        {
            var movies = await _movieRepository.GetAllMovieAsync();

            return movies.Select(m =>
            {
                var avarageScore = m.Votes.Any() ? m.Votes.Average(m => m.Score) : 0;
                var totalVotes = m.Votes.Sum(m => m.Score);
                return _mapping.ToMovieTitleResponse(m, avarageScore, totalVotes);
            });
        }

        public async Task<MovieDetailsResponse> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null) throw new MovieNotFoundException();

            var avarageScore = movie.Votes.Any() ? movie.Votes.Average(m => m.Score) : 0;
            var totalVotes = movie.Votes.Sum(m => m.Score);

            return _mapping.ToDetailsResponse(movie, avarageScore, totalVotes);
        }

        public async Task<bool> UpdateMovieAsync(int id, UpdateMovie updateMovie)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);

            if (movie == null) throw new MovieNotFoundException();

            movie.Title = updateMovie.Title;
            movie.Synops = updateMovie.Synops;
            movie.Classification = updateMovie.Classification;
            movie.Genres = updateMovie.Genres;
            movie.Duration = updateMovie.Duration;
            movie.Cast = updateMovie.Cast;
            movie.Directors = updateMovie.Directors;
            movie.ReleasedYear = updateMovie.ReleasedYear;

            movie.UpdatedAt = DateTime.Now;
            await _movieRepository.UpdateMovieAsync(movie);

            return true;
        }

        public async Task<bool> DeleteMovieAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null) throw new MovieNotFoundException();

            movie.Status = MovieStatus.Offline;
            movie.DeletedAt = DateTime.Now;
            await _movieRepository.UpdateMovieAsync(movie);

            return true;
        }
    }
}

