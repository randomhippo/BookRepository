using BookRepository.App.Domain;

namespace BookRepository.App.AppServices
{
    public class SingleBookResponse : BaseResponse
    {
        public SingleBookResponse(Book content)
        {
            Content = content;
        }

        public virtual Book Content { get; private set; }
    }
}
