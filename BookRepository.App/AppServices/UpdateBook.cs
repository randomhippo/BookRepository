using System.ComponentModel;
using BookRepository.App.DataAccess;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace BookRepository.App.AppServices
{
    public class UpdateBook
    {
        public class Request : BaseRequest<Response>
        {
            public string Id { get; set; }

            public BookData UpdatedContent { get; set; }

        }

        public class BookData
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
                var bookToModify = _context.Books.FirstOrDefault(b => b.PresentedId == request.Id.ToUpper());

                if (bookToModify == null)
                {
                    return new Response()
                    {
                        Result = System.Net.HttpStatusCode.NotFound
                    };
                }

                decimal.TryParse(request.UpdatedContent.Price, out var price);

                bookToModify.Title = request.UpdatedContent.Title;
                bookToModify.Description = request.UpdatedContent.Description;
                bookToModify.Author = request.UpdatedContent.Author;
                bookToModify.Genre = request.UpdatedContent.Genre;
                bookToModify.Price = price;
                bookToModify.Published = request.UpdatedContent.Published;

                _context.Books.Update(bookToModify);
                await _context.SaveChangesAsync();

                return new Response();
            }
        }
    }
}
