using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs.User;
using MoviesApi.Entities;
using MoviesApi.Enums.Movie;
using MoviesApi.Enums.User;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;
using MoviesApi.Mapping;
using MoviesApi.Repositories;
using System.Data;
using System.IO;

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

        public async Task<MovieResponse> CreateMovieAsync(CreateMovieRequest createMovieRequest)
        {
            var movie = _mapping.CreateMovieRequestToEntity(createMovieRequest);
            await _movieRepository.CreateMovieAsync(movie);
            return _mapping.ToResponse(movie);
        }

        public async Task<IEnumerable<MovieResponse>> GetAllMovieAsync()
        {
            var movies = await _movieRepository.GetAllMovieAsync();
            var moviesResponse = movies.Select(m => _mapping.ToResponse(m));
            return moviesResponse;
        }

        public async Task<MovieResponse> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null) throw new BadHttpRequestException("Movie not found");
            return _mapping.ToResponse(movie);
        }

        public async Task<bool> UpdateMovieAsync(int id, UpdateMovie updateMovie)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);

            if (movie == null) throw new BadHttpRequestException("Movie not Found");

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
            if (movie == null) throw new BadHttpRequestException("Movie not Found");

            movie.Status = MovieStatus.Offline;
            movie.DeletedAt = DateTime.Now;
            await _movieRepository.UpdateMovieAsync(movie);

            return true;
        }

    }
}

