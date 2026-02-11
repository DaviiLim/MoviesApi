using Domain.DTOs.Movie;
using Domain.Entities;


namespace Domain.Interfaces.Mappers
{
    public interface IMovieMapping
    {
        public Movie CreateMovieRequestToEntity(CreateMovieRequest createMovieRequest);
        MovieTitleResponse ToMovieTitleResponse(Movie movie, float avarageScore, float totalVotes);
        MovieDetailsResponse ToDetailsResponse(Movie movie, float avarageScore, float totalVotes);
    }
}
