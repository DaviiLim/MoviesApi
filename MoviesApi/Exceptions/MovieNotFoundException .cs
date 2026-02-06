namespace MoviesApi.Exceptions
{
    public class MovieNotFoundException : BusinessException
    {
        public MovieNotFoundException()
            : base("Movie not found.") { }
    }
}
