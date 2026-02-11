namespace Domain.Exceptions
{
    public class ForbiddenUserVoteException : BusinessException
    {
        public ForbiddenUserVoteException()
            : base("You don´t have permission to vote") { }
    }
}
