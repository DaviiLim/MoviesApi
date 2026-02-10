using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs.Pagination;
using MoviesApi.Entities;
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
            var movieByTitle = await _movieRepository.GetMovieByTitleAsync(createMovieRequest.Title);
            if (movieByTitle != null)
                throw new TitleAlreadyExistsException();

            var movie = _mapping.CreateMovieRequestToEntity(createMovieRequest);
            await _movieRepository.CreateMovieAsync(movie);
            return _mapping.ToDetailsResponse(movie, 0, 0);
        }

        public async Task<PaginationResponse<MovieTitleResponse>> GetAllMovieAsync(PaginationParams paginationParams, string? title, string? genre, string? directors, string? cast)
        {
            var movies = await _movieRepository.GetAllMovieAsync();
            var query = movies.AsQueryable();

            if (!string.IsNullOrWhiteSpace(title))
            {
                query = query.Where(m =>
                    m.Title.ToLower().Contains(title.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(genre))
            {
                query = query.Where(m =>
                    m.Genres.ToLower().Contains(genre.ToLower()));
            }

            if (!string.IsNullOrWhiteSpace(directors))
            {
                query = query.Where(m =>
                    m.Directors.Any(d =>
                        d.ToLower().Contains(directors.ToLower())));
            }

            if (!string.IsNullOrWhiteSpace(cast))
            {
                query = query.Where(m =>
                    m.Cast.Any(a =>
                        a.ToLower().Contains(cast.ToLower())));
            }

            var totalItems = query.Count();

            query = query
                .OrderByDescending(m => m.Votes.Count())
                .ThenBy(m => m.Title);

            var pagedMovies = query
                .Skip((paginationParams.PageNumber - 1) * paginationParams.PageSize)
                .Take(paginationParams.PageSize)
                .ToList();

            var items = pagedMovies.Select(m =>
            {
                var averageScore = m.Votes.Any()
                    ? m.Votes.Average(v => v.Score)
                    : 0;

                var totalVotes = m.Votes.Count;

                return _mapping.ToMovieTitleResponse(m, averageScore, totalVotes);
            });

            return new PaginationResponse<MovieTitleResponse>
            {
                Items = items,
                TotalItems = totalItems,
                PageNumber = paginationParams.PageNumber,
                PageSize = paginationParams.PageSize
            };
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
            await _movieRepository.DeleteMovieAsync(movie);

            return true;
        }
    }
}

