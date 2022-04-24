using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRepository.App.AppServices
{
    public class GetBooksPublishedInInterval
    {
        public class Request : BaseRequest<Response>
        {
            public int Year { get; set; }

            public int? Month { get; set; }

            public int? Day { get; set; }

            public DateTime From
            {
                get
                {
                    return new DateTime(Year, Month ?? 1, Day ?? 1);
                }
            }

            public DateTime To
            {
                get
                {
                    if (Day.HasValue)
                        return From.AddDays(1).Subtract(TimeSpan.FromTicks(1));
                    if (Month.HasValue)
                        return From.AddMonths(1).Subtract(TimeSpan.FromTicks(1));
                    return From.AddYears(1).Subtract(TimeSpan.FromTicks(1));
                }
            }
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
                    .Where(book => book.Published >= request.From && book.Published <= request.To)
                    .OrderBy(book => book.Published)
                    .ToListAsync(cancellationToken)
                };

                return response;
            }
        }
    }
}