using System.ComponentModel;
using System.Globalization;
using System.Text.Json.Serialization;
using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using FluentValidation;
using MediatR;

namespace BookRepository.App.AppServices
{
    public class CreateBook
    {
        public class Request : BaseRequest<Response>
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
            public Response(Book created) : base(created)
            {
            }
        }

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                RuleFor(c => c.Author).NotEmpty();
                RuleFor(c => c.Description).NotEmpty();
                RuleFor(c => c.Genre).NotEmpty();
                RuleFor(c => c.Title).NotEmpty();
                //Condtion should be reused in a extension method to avoid duplication
                RuleFor(c => c.Price).Must(x => decimal.TryParse(x, NumberStyles.Number, provider: CultureInfo.InvariantCulture, out var val) && val > 0).WithMessage("Price must be more than 0");
                RuleFor(c => c.Published).NotEmpty();
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
                decimal.TryParse( request.Price, NumberStyles.Number, provider: CultureInfo.InvariantCulture, out var price);

                var newBook = new Book
                {
                    Author = request.Author,
                    Description = request.Description,
                    Genre = request.Genre,
                    Price = price,
                    Published = request.Published,
                    Title = request.Title
                };
                
                await _context.Books.AddAsync(newBook, cancellationToken);
                await _context.SaveChangesAsync(cancellationToken);

                return new Response(newBook);
            }
        }
    }
}
