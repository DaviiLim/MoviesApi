using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;


namespace MoviesApi.Interfaces.Mappers
{
    public interface IMovieMapping
    {
        public Movie CreateMovieRequestToEntity(CreateMovieRequest createMovieRequest);
        public MovieResponse ToResponse(Movie movie);
    }
}
