using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRepository.App.AppServices
{
    public class GetBooksWithExactPrice
    {
        public class Request : BaseRequest<Response>
        {
            public decimal Price { get; set; }
        }

        public class Response : BaseResponse
        {
            public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.Price).NotEmpty();
            }
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
                    .Where(book => book.Price == request.Price)
                    .ToListAsync(cancellationToken)
                };

                return response;
            }
        }
    }
}
