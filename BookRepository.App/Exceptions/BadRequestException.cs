namespace BookRepository.App.Exceptions
{
    public class BadRequestException : CoreException
    {
        protected BadRequestException(string message)
            : base("Bad Request", message)
        {
        }
    }
}
