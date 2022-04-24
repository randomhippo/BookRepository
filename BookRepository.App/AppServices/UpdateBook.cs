using BookRepository.App.DataAccess;
using MediatR;

namespace BookRepository.App.AppServices
{
    public class UpdateBook
    {
        public class Request : BaseRequest<Response>
        {
            public string Id { get; set; }

            public BookData UpdatedContent { get; set; }

        }

        public class BookData
        {
            public string Author { get; set; }
            public string Description { get; set; }
            public string Genre { get; set; }
            public decimal Price { get; set; }
            public DateTime Published { get; set; }
            public string Title { get; set; }
        }

        public class Response : BaseResponse
        {
        }

        public class Handler : IRequestHandler<Request, Response>
        {
            private BooksContext _context;

            public Handler(BooksContext context)
            {
                _context = context;
            }

            public async Task<Response> Handle(Request request, CancellationToken cancellationToken)
            {
                var bookToModify = _context.Books.FirstOrDefault(b => b.PresentedId == request.Id);

                if (bookToModify == null)
                {
                    return new Response()
                    {
                        Result = System.Net.HttpStatusCode.NotFound
                    };
                }
                    
                bookToModify.Title = request.UpdatedContent.Title;
                bookToModify.Description = request.UpdatedContent.Description;
                bookToModify.Author = request.UpdatedContent.Author;
                bookToModify.Genre = request.UpdatedContent.Genre;
                bookToModify.Price = request.UpdatedContent.Price;
                bookToModify.Published = request.UpdatedContent.Published;

                _context.Books.Update(bookToModify);
                await _context.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
