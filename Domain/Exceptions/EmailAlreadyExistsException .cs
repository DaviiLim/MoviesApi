namespace Domain.Exceptions
{
    public class EmailAlreadyExistsException : BusinessException
    {
        public EmailAlreadyExistsException()
            :base("Email already exists.") { }
    }
}
