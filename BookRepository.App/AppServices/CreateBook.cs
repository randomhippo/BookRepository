using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using MediatR;

namespace BookRepository.App.AppServices
{
    public class CreateBook
    {
        public class Request : BaseRequest<Response>
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
                var newBook = new Book
                {
                    Author = request.Author,
                    Description = request.Description,
                    Genre = request.Genre,
                    Price = request.Price,
                    Published = request.Published,
                    Title = request.Title
                };
                await _context.Books.AddAsync(newBook);
                await _context.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
