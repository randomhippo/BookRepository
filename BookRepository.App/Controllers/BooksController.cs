using BookRepository.App.AppServices;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace BookRepository.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        private readonly IMediator _mediator;

        public BooksController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var response = await _mediator.Send(new GetAllBooks.Request());
            return Ok(response.Books);
        }

        [HttpGet("{SortProperty}")]
        public async Task<IActionResult> GetSorted([FromRoute] GetBooksSorted.Request request)
        {
            var response = await _mediator.Send(request);
            return Ok(response.Books);
        }

        [HttpGet("{property}/{value}")]
        public async Task<IActionResult> GetFiltered([FromRoute] string property, [FromRoute] string value)
        {
            var request = new GetBooksFiltered.Request
            {
                FilterProperty = property,
                FilterValue = value
            };
            var response = await _mediator.Send(request);

            return Ok(response.Books);
        }

        [HttpGet("price/{Price:decimal}")]
        public async Task<IActionResult> GetForSpecificPrice([FromRoute] GetBooksWithExactPrice.Request request)
        {
            var response = await _mediator.Send(request);

            return Ok(response.Books);
        }

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

        [HttpGet("price/{From:decimal}&{To:decimal}")]
        public async Task<IActionResult> GetBooksForPriceRange([FromRoute] GetBooksForPriceRange.Request request)
        {
            var response = await _mediator.Send(request);

            return Ok(response.Books);
        }

        [HttpPut()]
        public async Task<IActionResult> CreateBook([FromBody] CreateBook.Request request)
        {
            var response = await _mediator.Send(request);

            return Ok();
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] string id, [FromBody] UpdateBook.BookData bookdata)
        {
            var request = new UpdateBook.Request
            {
                Id = id,
                UpdatedContent = bookdata
            };

            var response = await _mediator.Send(request);
            return StatusCode((int)response.Result);
            
        }

        

        //public class UpdateBookRequest
        //{
        //    public string Id { get; set; }
        //    public BookData BookData { get; set; }
        //}
    }
}
