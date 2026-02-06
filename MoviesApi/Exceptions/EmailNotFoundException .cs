namespace MoviesApi.Exceptions
{
    public class EmailNotFoundException : BusinessException
    {
        public EmailNotFoundException()
            : base("Email not found.") { }
    }
}
