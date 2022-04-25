namespace BookRepository.App.Exceptions
{
    public abstract class CoreException : Exception
    {
        protected CoreException(string title, string message)
            : base(message) =>
            Title = title;

        public string Title { get; }
    }

}
