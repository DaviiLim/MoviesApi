using Moq;
using MoviesApi.DTOs.Vote;
using MoviesApi.Entities;
using MoviesApi.Exceptions;
using MoviesApi.Interfaces.Mappers;
using MoviesApi.Interfaces.Repositories;
using MoviesApi.Services;

namespace MovieApiTest.Services
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

            _voteMappingMock
                .Setup(m => m.CreateVoteRequestToEntity(request))
                .Returns(vote);

            _voteRepositoryMock
                .Setup(r => r.VoteAsync(It.IsAny<Vote>()))
                .ReturnsAsync(true);

            var result = await _voteService.VoteAsync(1, request);

            Assert.True(result);
            Assert.Equal(1, vote.UserId);

            _voteRepositoryMock.Verify(
                r => r.VoteAsync(It.IsAny<Vote>()),
                Times.Once);
        }

        [Fact]
        public async Task DeleteVoteAsync_WhenUserDoesNotExist_ShouldThrowException()
        {
            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(It.IsAny<int>()))
                .ReturnsAsync((User)null);

            await Assert.ThrowsAsync<UserNotFoundException>(() =>
                _voteService.DeleteVoteAsync(1, 1));
        }

        [Fact]
        public async Task DeleteVoteAsync_WhenVoteBelongsToAnotherUser_ShouldThrowException()
        {
            var vote = ValidVote(userId: 2);

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(1))
                .ReturnsAsync(ValidUser(1));

            _voteRepositoryMock
                .Setup(r => r.GetVoteByIdAsync(1))
                .ReturnsAsync(vote);

            await Assert.ThrowsAsync<ForbiddenUserVoteException>(() =>
                _voteService.DeleteVoteAsync(1, 1));
        }

        [Fact]
        public async Task DeleteVoteAsync_WhenValid_ShouldSoftDeleteAndReturnTrue()
        {
            var vote = ValidVote(userId: 1);

            _userRepositoryMock
                .Setup(r => r.GetUserByIdAsync(1))
                .ReturnsAsync(ValidUser(1));

            _voteRepositoryMock
                .Setup(r => r.GetVoteByIdAsync(1))
                .ReturnsAsync(vote);

            var result = await _voteService.DeleteVoteAsync(1, 1);

            Assert.True(result);
            Assert.NotNull(vote.DeletedAt);
        }
    }
}
