using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRepository.App.AppServices
{
    public class GetAllBooks
    {
        public class Request : BaseRequest<Response>
        {

        }

        public class Response : BaseResponse
        {
            public IEnumerable<Book> Books { get; set; }
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
                return new Response
                {
                    Books = await _context.Books.ToListAsync(cancellationToken)
                };

            }
        }
    }
}
