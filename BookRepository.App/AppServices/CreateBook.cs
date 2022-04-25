using System.ComponentModel;
using System.Globalization;
using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
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
            public string Author { get; set; }
            /// <summary>
            /// A description of the book.
            /// </summary>
            [DefaultValue("A description")]
            public string Description { get; set; }
            [DefaultValue("Fiction")]
            public string Genre { get; set; }
            
            [DefaultValue("12.34")]
            public string Price { get; set; }
            [DefaultValue("2011-11-11")]
            public DateTime Published { get; set; }
            [DefaultValue("Title here")]
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
                decimal.TryParse( request.Price, out var price);

                var newBook = new Book
                {
                    Author = request.Author,
                    Description = request.Description,
                    Genre = request.Genre,
                    Price = price,
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
