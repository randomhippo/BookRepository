using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRepository.App.AppServices
{
    public class GetBooksSorted
    {
        public class Request : BaseRequest<Response>
        {
            public string SortProperty { get; set; } = String.Empty;
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
                    Books = await ApplySort(request.SortProperty, _context, cancellationToken)
                };

                return response;
            }

            private async Task<IEnumerable<Book>> ApplySort(string sortProperty, BooksContext context, CancellationToken cancellationToken)
            {
                IQueryable<Book> queryable = sortProperty.ToLower() switch
                {
                    "id" => context.Books.OrderBy(b => b.Id),
                    "author" => context.Books.OrderBy(b => b.Author),
                    "description" => context.Books.OrderBy(b => b.Description),
                    "genre" => context.Books.OrderBy(b => b.Genre),
                    "price" => context.Books.OrderBy(b => b.Price),
                    "title" => context.Books.OrderBy(b => b.Title),
                    "published" => context.Books.OrderBy(b => b.Published),
                    _ => context.Books,
                };
                return await queryable.ToListAsync(cancellationToken);
            }
        }
    }
}
