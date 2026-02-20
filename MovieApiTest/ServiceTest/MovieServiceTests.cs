using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Domain.DTOs.Movie;
using Domain.DTOs.Pagination;
using Domain.Entities;
using Domain.Enums.Movie;
using Domain.Interfaces.Mappers;
using Domain.Interfaces.Repositories;
using Domain.Services;
using FluentAssertions;
using NSubstitute;
using Xunit;

public class MovieServiceTests
{
    private readonly IMovieRepository _repository = Substitute.For<IMovieRepository>();
    private readonly IMovieMapping _mapping = Substitute.For<IMovieMapping>();

    private readonly MovieService _service;

    public MovieServiceTests()
    {
        _service = new MovieService(_repository, _mapping);
    }

    [Fact]
    public async Task CreateMovieAsync_ShouldFail_WhenTitleAlreadyExists()
    {
        var request = new CreateMovieRequest 
        {
            Title = "Batman",
            Synops = "Filme do Batman",
            Classification = "14",
            Genres = "Action",
            Duration = 120,
            Cast = new List<string> { "Actor 1" },
            Directors = new List<string> { "Director 1" },
            ReleasedYear = 2022
        };

        _repository.GetMovieByTitleAsync(request.Title)
            .Returns(new Movie
            {
                Title = "Batman",
                Synops = "Filme do Batman",
                Classification = "14",
                Genres = "Action",
                Duration = 120,
                Cast = new List<string> { "Actor 1" },
                Directors = new List<string> { "Director 1" },
                ReleasedYear = 2022
            });

        var result = await _service.CreateMovieAsync(request);

        result.IsFailed.Should().BeTrue();
        await _repository.DidNotReceive().CreateMovieAsync(Arg.Any<Movie>());
    }

    [Fact]
    public async Task CreateMovieAsync_ShouldCreateMovie()
    {
        var request = new CreateMovieRequest
        {
            Title = "Batman",
            Synops = "Filme do Batman",
            Classification = "14",
            Genres = "Action",
            Duration = 120,
            Cast = new List<string> { "Actor 1" },
            Directors = new List<string> { "Director 1" },
            ReleasedYear = 2022
        };

        _repository
            .CreateMovieAsync(Arg.Any<Movie>())
            .Returns(Task.CompletedTask);

        var result = await _service.CreateMovieAsync(request);

        result.IsSuccess.Should().BeTrue();
    }

    [Fact]
    public async Task GetMovieByIdAsync_ShouldFail_WhenMovieNotFound()
    {
        _repository.GetMovieByIdAsync(1)
            .Returns(null as Movie);

        var result = await _service.GetMovieByIdAsync(1);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task GetMovieByIdAsync_ShouldReturnMovie_WithCorrectAverage()
    {
        var movie = new Movie
        {
            Id = 1,
            Title = "Batman",
            Synops = "Filme do Batman",
            Classification = "14",
            Genres = "Action",
            Duration = 120,
            Cast = new List<string> { "Actor 1" },
            Directors = new List<string> { "Director 1" },
            ReleasedYear = 2022,
            CreatedAt = DateTime.UtcNow,
            Votes = new List<Vote>
    {
        new Vote { Score = 5 },
        new Vote { Score = 9 }
    }
        };

        _repository
            .GetMovieByIdAsync(1)
            .Returns(movie);

        _mapping
            .ToDetailsResponse(movie, 7, 2)
            .Returns(new MovieDetailsResponse
            {
                Id = 1,
                Title = "Batman",
                Synops = "Filme do Batman",
                Classification = "14",
                Genres = "Action",
                Duration = 120,
                Cast = new List<string> { "Actor 1" },
                Directors = new List<string> { "Director 1" },
                ReleasedYear = 2022,
                AvarageScore = 7,
                TotalVotes = 2
            });

        var result = await _service.GetMovieByIdAsync(1);

        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(1);
    }

    [Fact]
    public async Task GetAllMovieAsync_ShouldReturnPaginatedMovies_WithAverage()
    {
        var pagination = new PaginationParams { PageNumber = 1, PageSize = 10 };

        var movies = new List<Movie>
        {
            new Movie
            {
                Id = 1,
                Title = "Movie A",
                Genres = "Action",
                Synops = "Filme do Batman",
                Classification = "14",
                Directors = new List<string> { "Dir1" },
                Cast = new List<string> { "Actor1" },
                Votes = new List<Vote>
                {
                    new Vote { Score = 5 },
                    new Vote { Score = 7 }
                }
            }
        };

        var paged = new PaginationResponse<Movie>
        {
            PageNumber = 1,
            PageSize = 10,
            TotalItems = 1,
            Items = movies
        };

        _repository.GetAllMovieAsync(pagination, null, null, null, null)
            .Returns(paged);

        var result = await _service.GetAllMovieAsync(pagination, null, null, null, null);

        result.IsSuccess.Should().BeTrue();
        result.Value.TotalItems.Should().Be(1);
        result.Value.Items.First().AvarageScore.Should().Be(6);
        result.Value.Items.First().TotalVotes.Should().Be(2);
    }

    [Fact]
    public async Task UpdateMovieAsync_ShouldFail_WhenMovieNotFound()
    {
        _repository.GetMovieByIdAsync(1)
            .Returns(null as Movie);

        var result = await _service.UpdateMovieAsync(1, new UpdateMovie
        {
            Title = "NewTitle",
            Synops = "NewMovieAbout",
            Classification = "18",
            Genres = "Action",
            Duration = 120,
            Cast = ["Actor"],
            Directors = ["Director"],
            ReleasedYear = 2024
        }
            );

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task UpdateMovieAsync_ShouldUpdateMovie_WhenExists()
    {
        var movie = new Movie 
        {
            Id = 1,
            Title = "Movie A",
            Genres = "Action",
            Synops = "Filme do Batman",
            Classification = "14",
            Directors = new List<string> { "Dir1" },
            Cast = new List<string> { "Actor1" },
        };

        _repository.GetMovieByIdAsync(1)
            .Returns(movie);

        var update = new UpdateMovie
        {
            Title = "NewTitle",
            Synops = "NewMovieAbout",
            Classification = "18",
            Genres = "Action",
            Duration = 120,
            Cast = ["Actor"],
            Directors = ["Director"],
            ReleasedYear = 2024
        };

        var result = await _service.UpdateMovieAsync(1, update);

        result.IsSuccess.Should().BeTrue();
        movie.Title.Should().Be("NewTitle");

        await _repository.Received(1).UpdateMovieAsync(movie);
    }

    [Fact]
    public async Task DeleteMovieAsync_ShouldFail_WhenMovieNotFound()
    {
        _repository.GetMovieByIdAsync(1)
            .Returns(null as Movie);

        var result = await _service.DeleteMovieAsync(1);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteMovieAsync_ShouldSetOffline_WhenExists()
    {
        var movie = new Movie 
        {
            Title = "NewTitle",
            Synops = "NewMovieAbout",
            Classification = "18",
            Genres = "Action",
            Duration = 120,
            Cast = ["Actor"],
            Directors = ["Director"],
            ReleasedYear = 2024,
            Status = MovieStatus.Online 
        };

        _repository.GetMovieByIdAsync(1)
            .Returns(movie);

        var result = await _service.DeleteMovieAsync(1);

        result.IsSuccess.Should().BeTrue();
        movie.Status.Should().Be(MovieStatus.Offline);
        movie.DeletedAt.Should().NotBeNull();

        await _repository.Received(1).DeleteMovieAsync(movie);
    }
}