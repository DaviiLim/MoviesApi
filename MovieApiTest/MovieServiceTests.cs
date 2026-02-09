using Moq;
using MoviesApi.DTOs.Movie;
using MoviesApi.Entities;
using MoviesApi.Exceptions;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Interfaces.Services;
using MoviesApi.Mapping;
using MoviesApi.Services;

public class MovieServiceTests
{
    private readonly Mock<IMovieRepository> _movieRepositoryMock;
    private readonly MovieMapping _movieMapping;
    private readonly MovieService _movieService;

    public MovieServiceTests()
    {
        _movieRepositoryMock = new Mock<IMovieRepository>();
        _movieMapping = new MovieMapping();
        _movieService = new MovieService(_movieRepositoryMock.Object, _movieMapping);
    }

    [Fact]
    public async Task CreateMovieAsync_TitleAlreadyExists_ShouldThrowTitleAlreadyExistsException()
    {
        var request = CreateMovieRequest();

        var movie = CreateMovieEntity();

        _movieRepositoryMock
            .Setup(r => r.GetMovieByTitleAsync(request.Title))
            .ReturnsAsync(movie);

        await Assert.ThrowsAsync<TitleAlreadyExistsException>(() =>
            _movieService.CreateMovieAsync(request));

        _movieRepositoryMock.Verify(
            r => r.CreateMovieAsync(It.IsAny<Movie>()),
            Times.Never);
    }

    [Fact]
    public async Task CreateMovieAsync_TitleDoesntExist_ShouldReturnMovieTitleResponse()
    {
        var request = CreateMovieRequest();

        _movieRepositoryMock
            .Setup(r => r.GetMovieByTitleAsync(request.Title))
            .ReturnsAsync((Movie)null);

        _movieRepositoryMock
            .Setup(r => r.CreateMovieAsync(It.IsAny<Movie>()))
            .ReturnsAsync((Movie m) =>
            {
                m.Id = 1;
                return m;
            });

        var result = await _movieService.CreateMovieAsync(request);

        Assert.NotNull(result);
        Assert.IsType<MovieDetailsResponse>(result);
        Assert.Equal(request.Title, result.Title);

        _movieRepositoryMock.Verify(
            r => r.CreateMovieAsync(It.IsAny<Movie>()),
            Times.Once);
    }

    [Fact]
    public async Task GetMovieByIdAsync_WhenMovieExists_ShouldReturnMovieDetailsResponse()
    {
        var movie = CreateMovieEntity();

        _movieRepositoryMock
            .Setup(r => r.GetMovieByIdAsync(movie.Id))
            .ReturnsAsync(movie);

        var result = await _movieService.GetMovieByIdAsync(movie.Id);

        Assert.NotNull(result);
        Assert.IsType<MovieDetailsResponse>(result);

        _movieRepositoryMock.Verify(
            r => r.GetMovieByIdAsync(movie.Id),
            Times.Once);
    }

    [Fact]
    public async Task GetMovieByIdAsync_WhenMovieDoesntExist_ShouldThrowMovieNotFoundException()
    {
        _movieRepositoryMock
            .Setup(r => r.GetMovieByIdAsync(It.IsAny<int>()))
            .ReturnsAsync((Movie)null);

        await Assert.ThrowsAsync<MovieNotFoundException>(() =>
            _movieService.GetMovieByIdAsync(1));
    }

    [Fact]
    public async Task UpdateMovieAsync_WhenMovieExists_ShouldReturnTrue()
    {
        var movie = CreateMovieEntity();

        var update = new UpdateMovie
        {
            Title = "Updated",
            Synops = "Updated",
            Classification = "R",
            Genres = "Drama",
            Duration = 120,
            Cast = new List<string> { "Actor" },
            Directors = new List<string> { "Director" },
            ReleasedYear = 2024
        };

        _movieRepositoryMock
            .Setup(r => r.GetMovieByIdAsync(movie.Id))
            .ReturnsAsync(movie);

        _movieRepositoryMock
            .Setup(r => r.UpdateMovieAsync(It.IsAny<Movie>()))
            .ReturnsAsync(true);

        var result = await _movieService.UpdateMovieAsync(movie.Id, update);

        Assert.True(result);

        _movieRepositoryMock.Verify(
            r => r.UpdateMovieAsync(It.IsAny<Movie>()),
            Times.Once);
    }

    [Fact]
    public async Task DeleteUserAsync_WhenUserExists_ShouldReturnMovieTrue()
    {
        var movie = CreateMovieEntity();

        _movieRepositoryMock
            .Setup(r => r.GetMovieByIdAsync(movie.Id))
            .ReturnsAsync(movie);

        _movieRepositoryMock
            .Setup(r => r.DeleteMovieAsync(movie))
            .ReturnsAsync(true);

        var result = await _movieService.DeleteMovieAsync(movie.Id);
        Assert.IsType<bool>(result);
        Assert.True(result);

        _movieRepositoryMock.Verify(r =>
        r.DeleteMovieAsync(It.IsAny<Movie>()), Times.Once);
    }

    [Fact]
    public async Task DeleteMovieAsync_WhenMovieDoesntExists_ShouldReturnMovieNotFoundException()
    {
        var movie = CreateMovieEntity();

        _movieRepositoryMock
            .Setup(r => r.GetMovieByIdAsync(movie.Id))
            .ReturnsAsync((Movie)null);

        await Assert
            .ThrowsAsync<MovieNotFoundException>(() =>
            _movieService.DeleteMovieAsync(movie.Id));

        _movieRepositoryMock
            .Verify(r =>
            r.DeleteMovieAsync(It.IsAny<Movie>()), Times.Never);
    }

    private static CreateMovieRequest CreateMovieRequest() => new()
    {
        Title = "string",
        Synops = "stringstri",
        Classification = "R",
        Genres = "string",
        Duration = 1000,
        Cast = new List<string> { "string" },
        Directors = new List<string> { "string" },
        ReleasedYear = 2100
    };

    private static Movie CreateMovieEntity() => new()
    {
        Id = 1,
        Title = "string",
        Synops = "stringstri",
        Classification = "R",
        Genres = "string",
        Duration = 1000,
        Cast = new List<string> { "string" },
        Directors = new List<string> { "string" },
        ReleasedYear = 2100,
        CreatedAt = DateTime.UtcNow
    };
}
