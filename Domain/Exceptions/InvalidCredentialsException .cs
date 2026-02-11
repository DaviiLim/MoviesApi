namespace Domain.Exceptions
{
    public class InvalidCredentialsException : BusinessException
    {
        public InvalidCredentialsException()
            : base("Email or password is invalid.") { }
    }
}
