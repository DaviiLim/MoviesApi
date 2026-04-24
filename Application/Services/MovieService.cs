using Domain.Interfaces.Repositories;
using Domain.Entities;
using Domain.Enums.Movie;
using System.Data;
using Domain.DTOs.Pagination;
using FluentResults;
using Domain.Errors;
using Application.DTOs.Movie;
using Application.Interfaces.Mappers;
using Application.Interfaces.Services;

namespace Application.Services
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

        public async Task<Result<MovieDetailsResponse>> CreateMovieAsync(CreateMovieRequest createMovieRequest)
        {
            var movieByTitle = await _movieRepository.GetMovieByTitleAsync(createMovieRequest.Title);
            if (movieByTitle != null)
                return Result.Fail(new NotFoundError("This Title is already in use."));

            var movie = _mapping.CreateMovieRequestToEntity(createMovieRequest);
            await _movieRepository.CreateMovieAsync(movie);

            var movieDetailsResponse = _mapping.ToDetailsResponse(movie, 0, 0);
            return Result.Ok(movieDetailsResponse);
        }

        public async Task<Result<PaginationResponse<MovieTitleResponse>>> GetAllMovieAsync(
            PaginationParams paginationParams,
            string? title,
            string? genre,
            string? directors,
            string? cast)
        {
            var movies = await _movieRepository
                .GetAllMovieAsync(paginationParams, title, genre, directors, cast);

            var movieTitleResponse = new PaginationResponse<MovieTitleResponse>
            {
                PageNumber = movies.PageNumber,
                PageSize = movies.PageSize,
                TotalItems = movies.TotalItems,
                Items = movies.Items.Select(m => new MovieTitleResponse
                {
                    Id = m.Id,
                    Title = m.Title,
                    Genres = m.Genres,
                    Directors = m.Directors,
                    Cast = m.Cast,
                    AvarageScore = m.Votes!.Any()
                        ? m.Votes!.Average(v => v.Score)
                        : 0,
                    TotalVotes = m.Votes!.Count
                }).ToList()
            };

            return Result.Ok(movieTitleResponse);
        }

        public async Task<Result<MovieDetailsResponse>> GetMovieByIdAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);

            if (movie == null)
                return Result.Fail(new NotFoundError("Movie not found."));

            var votes = movie.Votes ?? new List<Vote>();

            var averageScore = votes.Any()
                ? votes.Average(v => v.Score)
                : 0;

            var totalVotes = votes.Count();

            var MovieDetailsResponse = _mapping.ToDetailsResponse(movie, averageScore, totalVotes);

            return Result.Ok(MovieDetailsResponse);
        }

        public async Task<Result<IEnumerable<MovieTitleResponse>>> GetAllMoviesVotedByUser(int userId)
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

            return Result.Ok(movieTitleResponse);
        }

        public async Task<Result> UpdateMovieAsync(int id, UpdateMovie updateMovie)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);

            if (movie == null)
                return Result.Fail(new NotFoundError("Movie not found."));

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

            return Result.Ok();
        }

        public async Task<Result> DeleteMovieAsync(int id)
        {
            var movie = await _movieRepository.GetMovieByIdAsync(id);
            if (movie == null)
                return Result.Fail(new NotFoundError("Movie not found."));

            movie.Status = MovieStatus.Offline;
            movie.DeletedAt = DateTime.Now;
            await _movieRepository.DeleteMovieAsync(movie);

            return Result.Ok();
        }
    }
}

