namespace MoviesApi.Exceptions
{
    public class TitleAlreadyExistsException : BusinessException
    {
        public TitleAlreadyExistsException()
            : base("Title already exists") { }
    }
}
