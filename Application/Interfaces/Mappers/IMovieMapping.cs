using Application.DTOs.Movie;
using Domain.Entities;


namespace Application.Interfaces.Mappers
{
    public interface IMovieMapping
    {
        Movie CreateMovieRequestToEntity(CreateMovieRequest createMovieRequest);
        MovieTitleResponse ToMovieTitleResponse(Movie movie, float averageScore, float totalVotes);
        MovieDetailsResponse ToDetailsResponse(Movie movie, float averageScore, float totalVotes);
    }
}
