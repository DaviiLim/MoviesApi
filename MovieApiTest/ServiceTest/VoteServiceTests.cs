using Domain.Interfaces.Repositories;
using Moq;
using Domain.DTOs.Vote;
using Domain.Entities;
using Domain.Exceptions;
using Domain.Interfaces.Mappers;
using Domain.Services;

namespace MovieApiTest.ServiceTest
{
    public class VoteServiceTests
    {
        private readonly Mock<IVoteRepository> _voteRepositoryMock;
        private readonly Mock<IUserRepository> _userRepositoryMock;
        private readonly Mock<IMovieRepository> _movieRepositoryMock;
        private readonly Mock<IVoteMapping> _voteMappingMock;

        private readonly VoteService _voteService;

        public VoteServiceTests()
        {
            _voteRepositoryMock = new Mock<IVoteRepository>();
            _userRepositoryMock = new Mock<IUserRepository>();
            _movieRepositoryMock = new Mock<IMovieRepository>();
            _voteMappingMock = new Mock<IVoteMapping>();

            _voteService = new VoteService(
                _voteMappingMock.Object,
                _voteRepositoryMock.Object,
                _userRepositoryMock.Object,
                _movieRepositoryMock.Object);
        }

        private CreateVoteRequest VoteRequest(int movieId = 1, int score = 5)
            => new()
            {
                MovieId = movieId,
                Score = score
            };

        private User ValidUser(int id = 1)
            => new()
            {
                Id = id,
                Name = "User",
                Email = "user@test.com"
            };

        private Movie ValidMovie(int id = 1)
            => new()
            {
                Id = id,
                Title = "Movie"
            };

        private Vote ValidVote(int id = 1, int userId = 1)
            => new()
            {
                Id = id,
                UserId = userId,
                MovieId = 1
            };

        [Fact]
        public async Task VoteAsync_WhenUserDoesNotExist_ShouldThrowException()
        {
            var request = VoteRequest();

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _voteService.VoteAsync(1, request));
        }

        [Fact]
        public async Task VoteAsync_WhenMovieDoesNotExist_ShouldThrowException()
        {
            var request = VoteRequest();

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(1))
                .ReturnsAsync(ValidUser());

            _movieRepositoryMock
                .Setup(r => r.GetMovieByIdAsync(request.MovieId))
                .ReturnsAsync((Movie)null);

            await Assert.ThrowsAsync<MovieNotFoundException>(() =>
                _voteService.VoteAsync(1, request));
        }

        [Fact]
        public async Task VoteAsync_WhenValid_ShouldCreateVoteAndReturnTrue()
        {
            var request = VoteRequest();
            var vote = ValidVote();

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(1))
                .ReturnsAsync(ValidUser());

            _movieRepositoryMock
                .Setup(r => r.GetMovieByIdAsync(request.MovieId))
                .ReturnsAsync(ValidMovie());

            _voteRepositoryMock
                .Setup(r => r.ExistsVoteAsync(1, request.MovieId))
                .ReturnsAsync((Vote)null);

            _voteMappingMock
                .Setup(m => m.CreateVoteRequestToEntity(request))
                .Returns(vote);

            _voteRepositoryMock
                .Setup(r => r.AddAsync(It.IsAny<Vote>()))
                .Returns(Task.CompletedTask);

            _voteRepositoryMock
                .Setup(r => r.SaveChangesAsync())
                .Returns(Task.CompletedTask);

            var result = await _voteService.VoteAsync(1, request);

            Assert.True(result);
            Assert.Equal(1, vote.UserId);

            _voteRepositoryMock.Verify(
                r => r.AddAsync(It.IsAny<Vote>()),
                Times.Once);

            _voteRepositoryMock.Verify(
                r => r.SaveChangesAsync(),
                Times.Once);
        }
    }
}
