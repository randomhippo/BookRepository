using BookRepository.App.Domain;
using Microsoft.AspNetCore.Mvc;

namespace BookRepository.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BooksController : ControllerBase
    {
        public readonly Book book1 = new Book
        {
            Id = "B1",
            Author = "Kutner, Joe",
            Title = "Deploying with JRuby",
            Genre = "Computer",
            Price = 33.00M,
            Published = new DateTime(2012, 8, 15),
            Description = "Deploying with JRuby is the missing link between enjoying JRuby and using it in the real world to build high-performance, scalable applications."
        };

        public readonly Book book2 = new Book
        {
            Id = "B2",
            Author = "Ralls, Kim",
            Title = "Midnight Rain",
            Genre = "Fantasy",
            Price = 5.95M,
            Published = new DateTime(2000, 12, 16),
            Description = "A former architect battles corporate zombies, an evil sorceress, and her own childhood to become queen of the world."
        };

        [HttpGet()]
        public async Task<IActionResult> GetAll()
        {
            var books = new List<Book>() { book1, book2 };

            return Ok(books);
        }

        [HttpGet("{property}")]
        public async Task<IActionResult> GetSorted([FromRoute] string property)
        {
            var books = new List<Book>() { book1, book2 };
            switch (property.ToLower())
            {
                case "id":
                    return Ok(books.OrderBy(b => b.Id));
                case "author":
                    return Ok(books.OrderBy(b => b.Author));
                case "genre":
                    return Ok(books.OrderBy(b => b.Genre));
                case "price":
                    return Ok(books.OrderBy(b => b.Price));
                default:
                    return BadRequest("Invalid sort option or property not supported in POC code.");

            }
        }

        [HttpGet("{property}/{value}")]
        public async Task<IActionResult> GetFiltered([FromRoute] string property, [FromRoute] string value)
        {
            var books = new List<Book>() { book1, book2 };


            switch (property.ToLower())
            {
                case "id":
                    return Ok(books.Where(b => b.Id.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(b => b.Id));
                case "author":
                    return Ok(books.Where(b => b.Author.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(b => b.Author));
                case "genre":
                    return Ok(books.Where(b => b.Genre.Contains(value, StringComparison.OrdinalIgnoreCase)).OrderBy(b => b.Genre));
                default:
                    return BadRequest("Invalid sort option or property not supported in POC code.");

            }
        }

        [HttpGet("price/{Price:decimal}")]
        public async Task<IActionResult> GetForSpecificPrice([FromRoute] PriceRequest request)
        {
            return Ok($"Return books costing exactly {request.Price}");
        }

        [HttpGet("published/{Year:int}/{Month:int}/{Day:int}")]
        public async Task<IActionResult> GetForPublishedDate([FromRoute] PublishedRangeRequest request)
        {
            return Ok($"Return books published {request.Year}-{request.Month}-{request.Day}");
        }

        [HttpGet("published/{year:int}/{month:int}")]
        public async Task<IActionResult> GetForPublishedMonth([FromRoute] int year, [FromRoute] int month)
        {
            return Ok($"Return books published {year}-{month}");
        }

        [HttpGet("published/{year:int}")]
        public async Task<IActionResult> GetForPublishedYear([FromRoute] int year)
        {
            return Ok($"Return books published {year}");
        }


        [HttpGet("price/{From:decimal}&{To:decimal}")]
        public async Task<IActionResult> GetPriceRange([FromRoute] PriceRangeRequest request)
        {
            return Ok($"Return books costing between {request.From} and {request.To}");
        }

        [HttpPut()]
        public async Task<IActionResult> CreateBook([FromBody] BookData request)
        {
            return BadRequest("When done, a book will be created if the request body is valid.");
        }

        [HttpPost("{id}")]
        public async Task<IActionResult> UpdateBook([FromRoute] string id,  [FromBody] BookData bookdata)
        {
            return BadRequest("When done, a book will be created if the request body is valid.");
        }

        public class BookData
        {
             public string Author      { get; set;}
             public string Description { get; set;}
             public string Genre       { get; set;}
             public string Price       { get; set;}
             public string Published   { get; set;}
             public string Title { get; set; }
        }

        public class UpdateBookRequest
        {
            public string Id { get; set;}
            public BookData BookData { get; set;}
        }

        public class PriceRangeRequest
        {
            public decimal From { get; set; }

            public decimal To { get; set; }
        }

        public class PriceRequest
        {
            public decimal Price { get; set; }

        }

        public class PublishedRangeRequest
        {
            public int Year { get; set; }

            public int? Month { get; set; }

            public int? Day { get; set; }

        }
    }
}
