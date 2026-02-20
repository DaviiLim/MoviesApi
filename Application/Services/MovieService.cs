using Domain.Interfaces.Repositories;
using Domain.DTOs.Movie;
using Domain.Entities;
using Domain.Enums.Movie;
using Domain.Exceptions;
using Domain.Interfaces.Mappers;
using Domain.Interfaces.Services;
using System.Data;
using Domain.DTOs.Pagination;

namespace Domain.Services
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
            var movieByTitle = await _movieRepository.GetMovieByTitleAsync(createMovieRequest.Title);
            if (movieByTitle != null)
                throw new TitleAlreadyExistsException();

            var movie = _mapping.CreateMovieRequestToEntity(createMovieRequest);
            await _movieRepository.CreateMovieAsync(movie);
            return _mapping.ToDetailsResponse(movie, 0, 0);
        }

        public async Task<PaginationResponse<MovieTitleResponse>> GetAllMovieAsync(
            PaginationParams paginationParams,
            string? title,
            string? genre,
            string? directors,
            string? cast)
        {
            var movies = await _movieRepository
                .GetAllMovieAsync(paginationParams, title, genre, directors, cast);

            var response = new PaginationResponse<MovieTitleResponse>
            {
                PageNumber = movies.PageNumber,
                PageSize = movies.PageSize,
                TotalItems = movies.TotalItems,
                Items = movies.Items.Select(m => new MovieTitleResponse
                {
                    Id = m.Id,
                    Title = m.Title,
                    AvarageScore = m.Votes.Any()
                        ? m.Votes.Average(v => v.Score)
                        : 0,
                    TotalVotes = m.Votes.Count
                }).ToList()
            };

            return response;
        }

        public async Task<MovieDetailsResponse> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);

            if (movie == null)
                throw new MovieNotFoundException();

            var votes = movie.Votes ?? new List<Vote>();

            var averageScore = votes.Any()
                ? votes.Average(v => v.Score)
                : 0;

            var totalVotes = votes.Count();

            return _mapping.ToDetailsResponse(movie, averageScore, totalVotes);
        }

        public async Task<IEnumerable<MovieTitleResponse>> GetAllUserMovies(int userId) 
        {
            var movies = await _movieRepository.GetAllMoviesVotedByUser(userId);
            var movieTitleResponse = movies.Select(m =>
            {
                var votes = m.Votes ?? new List<Vote>();

                var averageScore = votes.Any()
                    ? votes.Average(v => v.Score)
                    : 0;

                var totalVotes = votes.Count();

                return _mapping.ToMovieTitleResponse(m, averageScore, totalVotes);
            });
                return movieTitleResponse;
        }

        public async void UpdateMovieAsync(int id, UpdateMovie updateMovie)
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

            _movieRepository.UpdateMovieAsync(movie);
        }

        public async void DeleteMovieAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null) throw new MovieNotFoundException();

            movie.Status = MovieStatus.Offline;
            movie.DeletedAt = DateTime.Now;
            _movieRepository.DeleteMovieAsync(movie);
        }
    }
}

