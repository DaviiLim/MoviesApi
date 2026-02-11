namespace Domain.Exceptions
{
    public class TitleAlreadyExistsException : BusinessException
    {
        public TitleAlreadyExistsException()
            : base("Title already exists") { }
    }
}
