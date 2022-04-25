using BookRepository.App.AppServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookRepository.App.Controllers
{
    /// <summary>
    /// API to manage a collection of books. 
    /// </summary>
    /// <remarks>Note that the database is reset between each run. The SqlLite database has the location %localappdata%\books.db</remarks>
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="mediator">Injected mediator</param>
        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        /// <summary>
        /// Gets all books currently stored in the database
        /// </summary>
        /// <returns>All books not sorted on anything</returns>
        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllBooks.Request());
            return Ok(response.Books);
        }

        /// <summary>
        /// Gets all books currently stored sorted by the specified field
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpGet("{SortProperty}")]
        public async Task<IActionResult> GetSorted([FromRoute] GetBooksSorted.Request request)
        {
            var response = await _mediator.Send(request);
            return Ok(response.Books);
        }

        /// <summary>
        /// Returns the books filtered on the specified property
        /// </summary>
        /// <param name="filterOn">The book property to filter on. 
        /// The following values are accepted (case insensitive):
        /// id, author, title, description, genre
        /// 
        /// For published and price, use corresponding endpoints
        /// </param>
        /// <param name="filterExpression">The value used in the filter</param>
        /// <returns>Returns matching books sorted according the specified property.</returns>
        [HttpGet("{filterOn}/{filterExpression}")]
        public async Task<IActionResult> GetFiltered([FromRoute] string filterOn, [FromRoute] string filterExpression)
        {
            var request = new GetBooksFiltered.Request
            {
                FilterProperty = filterOn,
                FilterValue = filterExpression
            };
            var response = await _mediator.Send(request);

            return Ok(response.Books);
        }

        /// <summary>
        /// Get books costing exactly this amount
        /// </summary>
        /// <param name="price">The amount. Use . as decimal separator</param>
        /// <returns></returns>
        [HttpGet("price/{Price:decimal}")]
        public async Task<IActionResult> GetForSpecificPrice([FromRoute] GetBooksWithExactPrice.Request price)
        {
            var response = await _mediator.Send(price);

            return Ok(response.Books);
        }

        /// <summary>
        /// Gets books published on the specific calendar day
        /// </summary>
        /// <param name="year">The publish year</param>
        /// <param name="month">The publish month</param>
        /// <param name="day">The publish day</param>
        /// <returns></returns>
        [HttpGet("published/{Year:int}/{Month:int}/{Day:int}")]
        public async Task<IActionResult> GetForPublishedDate([FromRoute] int year, [FromRoute] int month, [FromRoute] int day)
        {
            var request = new GetBooksPublishedInInterval.Request
            {
                Year = year,
                Month = month,
                Day = day
            };
            var response = await _mediator.Send(request);

            return Ok(response.Books);
        }

        /// <summary>
        /// Gets books published in the specific month
        /// </summary>
        /// <param name="year">The publish year</param>
        /// <param name="month">The publish month</param>
        /// <returns></returns>
        [HttpGet("published/{year:int}/{month:int}")]
        public async Task<IActionResult> GetForPublishedMonth([FromRoute] int year, [FromRoute] int month)
        {
            var request = new GetBooksPublishedInInterval.Request
            {
                Year = year,
                Month = month
            };
            var response = await _mediator.Send(request);

            return Ok(response.Books);
        }

        /// <summary>
        /// Gets books published in the specified year
        /// </summary>
        /// <param name="year">The publish year</param>
        /// <returns></returns>
        [HttpGet("published/{year:int}")]
        public async Task<IActionResult> GetForPublishedYear([FromRoute] int year)
        {
            var request = new GetBooksPublishedInInterval.Request
            {
                Year = year
            };
            var response = await _mediator.Send(request);

            return Ok(response.Books);
        }

        /// <summary>
        /// Get books with a price in the specified range
        /// </summary>
        /// <param name="priceRange">The amount. Use . as decimal separator</param>
        /// <returns></returns>
        [HttpGet("price/{From:decimal}&{To:decimal}")]
        public async Task<IActionResult> GetBooksForPriceRange([FromRoute] GetBooksForPriceRange.Request priceRange)
        {
            var response = await _mediator.Send(priceRange);

            return Ok(response.Books);
        }

        /// <summary>
        /// Creates a new book and stores it in the database
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPut()]
        public async Task<IActionResult> CreateBook([FromBody] CreateBook.Request request)
        {
            var response = await _mediator.Send(request);

            //Find a way get the url without hardcoding and/or breaking separation of concerns.
            return Created($"/api/books/id/{response.Content.PresentedId}", response.Content);
        }

        /// <summary>
        /// Updates the specified book
        /// </summary>
        /// <param name="id">Id of entity to update.</param>
        /// <param name="bookdata"></param>
        /// <returns></returns>
        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] string id, [FromBody] UpdateBook.BookData bookdata)
        {
            var request = new UpdateBook.Request
            {
                Id = id,
                UpdatedContent = bookdata
            };

            var response = await _mediator.Send(request);
            

            return StatusCode((int)response.Result, response.Content);
            
        }
    }
}
