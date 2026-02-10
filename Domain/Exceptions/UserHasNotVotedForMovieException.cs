namespace MoviesApi.Exceptions
{
    public class UserHasNotVotedForMovieException : BusinessException

    {
        public UserHasNotVotedForMovieException()
            : base ("User has not voted for this movie.") {}
    }
}
