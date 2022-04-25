using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;
using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using BookRepository.App.Exceptions;
using FluentValidation;
using MediatR;

namespace BookRepository.App.AppServices
{
    public class UpdateBook
    {
        public class Request : BaseRequest<Response>
        {
            public string Id { get; set; } = null!;

            public BookData UpdatedContent { get; set; } = null!;

        }

        public class BookData
        {
            /// <summary>
            /// The author
            /// </summary>
            [DefaultValue("Someone")]
            public string Author { get; set; } = null!;
            /// <summary>
            /// A description of the book.
            /// </summary>
            [DefaultValue("A description")]
            public string Description { get; set; } = null!;
            [DefaultValue("Fiction")]
            public string Genre { get; set; } = null!;

            [DefaultValue("12.34")]
            public string Price { get; set; } = null!;
            
            [DefaultValue("2011-11-11")]
            [JsonPropertyName("publish_date")]
            public DateTime Published { get; set; }
            
            [DefaultValue("Title here")]
            public string Title { get; set; } = null!;
        }

        public class Response : SingleBookResponse
        {
            public Response(Book updated) : base(updated)
            {
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(c => c.Id).NotEmpty();
                RuleFor(c => c.UpdatedContent).NotEmpty();
                When(c => c.UpdatedContent != null, () =>
                {
                    RuleFor(c => c.UpdatedContent.Author).NotEmpty();
                    RuleFor(c => c.UpdatedContent.Description).NotEmpty();
                    RuleFor(c => c.UpdatedContent.Genre).NotEmpty();
                    RuleFor(c => c.UpdatedContent.Title).NotEmpty();
                    RuleFor(c => c.UpdatedContent.Price).Must(x => decimal.TryParse(x, NumberStyles.Number, provider: CultureInfo.InvariantCulture, out var val) && val > 0)
                    .WithMessage("Price must be more than 0");
                    RuleFor(c => c.UpdatedContent.Published).NotEmpty();
                });
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
                var bookToModify = _context.Books.FirstOrDefault(b => b.PresentedId == request.Id.ToUpper());

                if (bookToModify == null)
                {
                    throw new NotFoundException("The specified book was not found");
                }

                decimal.TryParse(request.UpdatedContent.Price, NumberStyles.Number, provider: CultureInfo.InvariantCulture, out var price);

                bookToModify.Title = request.UpdatedContent.Title;
                bookToModify.Description = request.UpdatedContent.Description;
                bookToModify.Author = request.UpdatedContent.Author;
                bookToModify.Genre = request.UpdatedContent.Genre;
                bookToModify.Price = price;
                bookToModify.Published = request.UpdatedContent.Published;

                _context.Books.Update(bookToModify);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response(bookToModify);
            }
        }
    }
}
