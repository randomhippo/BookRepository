using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRepository.App.AppServices
{
    public class GetBooksFiltered
    {
        public class Request : BaseRequest<Response>
        {
            public string FilterProperty { get; set; } = null!;

            public string FilterValue { get; set; } = null!;
        }

        public class Response : BaseResponse
        {
            public IEnumerable<Book> Books { get; set; } = Enumerable.Empty<Book>();
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(request => request.FilterProperty).IsEnumName(typeof(SupportedStringFilterProperties), caseSensitive: false);
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
                    Books = await ApplyFilter(request.FilterProperty, request.FilterValue, _context, cancellationToken)
                };

                return response;
            }

            private async Task<IEnumerable<Book>> ApplyFilter(string property, string value, BooksContext context, CancellationToken cancellationToken)
            {
                IQueryable<Book> queryable = property.ToLower() switch
                {
                    //Since string.Contains will always be compared in a case sensitive way for current version of EF core, the use of EF.Functions is required instead
                    "id" => context.Books.Where(b => EF.Functions.Like(b.PresentedId, $"%{value}%")).OrderBy(b => b.PresentedId),
                    "author" => context.Books.Where(b => EF.Functions.Like(b.Author, $"%{value}%")).OrderBy(b => b.Author),
                    "description" => context.Books.Where(b => EF.Functions.Like(b.Description, $"%{value}%")).OrderBy(b => b.Description),
                    "genre" => context.Books.Where(b => EF.Functions.Like(b.Genre, $"%{value}%")).OrderBy(b => b.Genre),
                    "title" => context.Books.Where(b => EF.Functions.Like(b.Title, $"%{value}%")).OrderBy(b => b.Title),
                    _ => context.Books,
                };
                
                var query = queryable.ToQueryString();
                return await queryable.ToListAsync(cancellationToken);
            }
        }
    }
}
