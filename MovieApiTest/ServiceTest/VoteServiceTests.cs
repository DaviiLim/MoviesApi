
using Domain.DTOs.Vote;
using Domain.Entities;
using Domain.Enums.Vote;
using Domain.Interfaces.Mappers;
using Domain.Interfaces.Repositories;
using Domain.Services;
using FluentAssertions;
using NSubstitute;

public class VoteServiceTests
{
    private readonly IVoteRepository _voteRepository = Substitute.For<IVoteRepository>();
    private readonly IUserRepository _userRepository = Substitute.For<IUserRepository>();
    private readonly IMovieRepository _movieRepository = Substitute.For<IMovieRepository>();
    private readonly IVoteMapping _mapping = Substitute.For<IVoteMapping>();

    private readonly VoteService _service;

    public VoteServiceTests()
    {
        _service = new VoteService(
            _mapping,
            _voteRepository,
            _userRepository,
            _movieRepository);
    }

    [Fact]
    public async Task VoteAsync_ShouldFail_WhenUserNotFound()
    {
        _userRepository.GetUserByIdAsync(1)
              .Returns(null as User);

        var result = await _service.VoteAsync(1, new CreateVoteRequest());

        result.IsFailed.Should().BeTrue();
        await _voteRepository.DidNotReceive().AddAsync(Arg.Any<Vote>());
    }

    [Fact]
    public async Task VoteAsync_ShouldFail_WhenMovieNotFound()
    {
        _userRepository.GetUserByIdAsync(1)
            .Returns(new User
            {
                Email = "novo@email.com",
                Password = "123456",
                Name = "Davi"
            });

        _movieRepository.GetMovieByIdAsync(10)
             .Returns(null as Movie);

        var request = new CreateVoteRequest { MovieId = 10 };

        var result = await _service.VoteAsync(1, request);

        result.IsFailed.Should().BeTrue();
        await _voteRepository.DidNotReceive().AddAsync(Arg.Any<Vote>());
    }

    [Fact]
    public async Task VoteAsync_ShouldCreateVote_WhenNotExists()
    {
        var userId = 1;

        _userRepository.GetUserByIdAsync(userId)
            .Returns(new User
            {
                Email = "novo@email.com",
                Password = "123456",
                Name = "Davi"
            });

        _movieRepository.GetMovieByIdAsync(10)
            .Returns(new Movie
            {
                Title = "NewTitle",
                Synops = "NewMovieAbout",
                Classification = "18",
                Genres = "Action",
                Duration = 120,
                Cast = ["Actor"],
                Directors = ["Director"],
                ReleasedYear = 2024
            });

        _voteRepository.ExistsVoteAsync(userId, 10)
            .Returns(null as Vote);

        var voteEntity = new Vote { MovieId = 10, Score = 5 };

        _mapping.CreateVoteRequestToEntity(Arg.Any<CreateVoteRequest>())
            .Returns(voteEntity);

        var request = new CreateVoteRequest
        {
            MovieId = 10,
            Score = 5
        };

        var result = await _service.VoteAsync(userId, request);

        result.IsSuccess.Should().BeTrue();
        await _voteRepository.Received(1).AddAsync(voteEntity);
        await _voteRepository.Received(1).SaveChangesAsync();
        voteEntity.UserId.Should().Be(userId);
    }

    [Fact]
    public async Task VoteAsync_ShouldUpdateVote_WhenAlreadyExists()
    {
        var userId = 1;

        var user = new User
        {
            Id = userId,
            Email = "novo@email.com",
            Password = "123456",
            Name = "Davi"
        };

        var movie = new Movie
        {
            Id = 10,
            Title = "NewTitle",
            Synops = "NewMovieAbout",
            Classification = "18",
            Genres = "Action",
            Duration = 120,
            Cast = new List<string> { "Actor" },
            Directors = new List<string> { "Director" },
            ReleasedYear = 2024,
            CreatedAt = DateTime.UtcNow
        };

        var existingVote = new Vote
        {
            MovieId = 10,
            UserId = userId,
            Score = 3,
            Status = VoteStatus.Inactive
        };

        _userRepository.GetUserByIdAsync(userId)
            .Returns(user);

        _movieRepository.GetMovieByIdAsync(10)
            .Returns(movie);

        _voteRepository.ExistsVoteAsync(userId, 10)
            .Returns(existingVote);

        var request = new CreateVoteRequest
        {
            MovieId = 10,
            Score = 9
        };

        var result = await _service.VoteAsync(userId, request);

        result.IsSuccess.Should().BeTrue();

        existingVote.Score.Should().Be(9);
        existingVote.Status.Should().Be(VoteStatus.Active);

        await _voteRepository.DidNotReceive().AddAsync(Arg.Any<Vote>());
        await _voteRepository.Received(1).SaveChangesAsync();
    }

    [Fact]
    public async Task DeleteVoteAsync_ShouldFail_WhenVoteNotFound()
    {
        var userId = 1;
        var movieId = 10;

        _userRepository.GetUserByIdAsync(userId)
            .Returns(new User
            {
                Email = "novo@email.com",
                Password = "123456",
                Name = "Davi"
            });

        _movieRepository.GetMovieByIdAsync(movieId)
            .Returns(new Movie
            {
                Id = 1,
                Title = "Movie A",
                Genres = "Action",
                Synops = "Filme do Batman",
                Classification = "14",
                Directors = new List<string> { "Dir1" },
                Cast = new List<string> { "Actor1" },
            });

        _voteRepository.GetAllVotesAsync()
            .Returns(new List<Vote>());

        var result = await _service.DeleteVoteAsync(userId, movieId);

        result.IsFailed.Should().BeTrue();
    }

    [Fact]
    public async Task DeleteVoteAsync_ShouldDeactivateVote_WhenExists()
    {
        var userId = 1;
        var movieId = 10;

        _userRepository.GetUserByIdAsync(userId)
            .Returns(new User
            {
                Email = "novo@email.com",
                Password = "123456",
                Name = "Davi"
            });

        _movieRepository.GetMovieByIdAsync(movieId)
            .Returns(new Movie
            {
                Id = 1,
                Title = "Movie A",
                Genres = "Action",
                Synops = "Filme do Batman",
                Classification = "14",
                Directors = new List<string> { "Dir1" },
                Cast = new List<string> { "Actor1" },
            });

        var vote = new Vote
        {
            MovieId = movieId,
            UserId = userId,
            Status = VoteStatus.Active
        };

        _voteRepository.GetAllVotesAsync()
            .Returns(new List<Vote> { vote });

        var result = await _service.DeleteVoteAsync(userId, movieId);

        result.IsSuccess.Should().BeTrue();
        vote.Status.Should().Be(VoteStatus.Inactive);
        vote.DeletedAt.Should().NotBeNull();

        await _voteRepository.Received(1).DeleteVoteAsync(vote);
    }
}