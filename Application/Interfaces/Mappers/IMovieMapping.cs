using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;


namespace MoviesApi.Interfaces.Mappers
{
    public interface IMovieMapping
    {
        public Movie CreateMovieRequestToEntity(CreateMovieRequest createMovieRequest);
        MovieTitleResponse ToMovieTitleResponse(Movie movie, float avarageScore, float totalVotes);
        MovieDetailsResponse ToDetailsResponse(Movie movie, float avarageScore, float totalVotes);
    }
}
