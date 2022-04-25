using BookRepository.App.DataAccess;
using BookRepository.App.Domain;
using FluentValidation;
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

        public class Validator : AbstractValidator<Request>
        {
            public Validator()
            {
                
                RuleFor(request => request.Year).InclusiveBetween(1, 9999);
                RuleFor(request => request.Month).InclusiveBetween(1, 12).When(request => request.Month.HasValue);
                
                When(request => request.Day.HasValue, () =>
                {
                    RuleFor(request => request.Month).NotNull();
                    RuleFor(request => request.Day)
                        .Must((request, day) => day >= 1 && day <= DateTime.DaysInMonth(request.Year, request.Month.Value))
                        .When(request => request.Year >= 1 && request.Year <= 9999 && request.Month >= 1 && request.Month <= 12)
                        .WithMessage("Day must be between 1 and the numbers of day in that month/year");
                });

                

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