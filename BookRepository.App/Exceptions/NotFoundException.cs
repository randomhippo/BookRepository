namespace BookRepository.App.Exceptions
{
    public class NotFoundException : CoreException
    {
        public NotFoundException(string message)
            : base("Not Found", message)
        {
        }
    }
}
