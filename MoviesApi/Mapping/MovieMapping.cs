


using MoviesApi.DTOs.Movie;
using MoviesApi.DTOs.User;
using MoviesApi.Entities;
using MoviesApi.Interfaces.Mappers;

namespace MoviesApi.Mapping
{
    public class MovieMapping : IMovieMapping
    {
        public Movie CreateMovieRequestToEntity(CreateMovieRequest createMovieRequest)
        {
            return new Movie
            {
                Title = createMovieRequest.Title,
                Synops = createMovieRequest.Synops,
                Classification = createMovieRequest.Classification,
                Genres = createMovieRequest.Genres,
                Duration = createMovieRequest.Duration,
                Cast = createMovieRequest.Cast,
                Directors = createMovieRequest.Directors

            };
        }

        public MovieResponse ToResponse(Movie movie)
        {
            return new MovieResponse
            {
                Id = movie.Id,
                Title = movie.Title,
                Synops = movie.Synops,
                Classification = movie.Classification,
                Genres = movie.Genres,
                Duration = movie.Duration,
                Cast = movie.Cast,
                Directors = movie.Directors
            };
        }
    }
}
