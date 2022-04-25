using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRepository.App.AppServices
{
    public class GetBooksForPriceRange
    {
        public class Request : BaseRequest<Response>
        {
            /// <summary>
            /// The lower end of the price range. Should be less than or equal to the upper end.
            /// </summary>
            public decimal From { get; set; }

            /// <summary>
            /// The upper end of the price range. Should be greater than or equal to the lower end.
            /// </summary>
            public decimal To { get; set; }
        }

        public class Response : BaseResponse
        {
            public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();
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
                var response = new Response
                {
                    Books = await _context.Books
                    .Where(book => book.Price >= request.From && book.Price >= request.To)
                    .OrderBy(book => book.Price)
                    .ToListAsync(cancellationToken)
                };

                return response;
            }
        }
    }
}
