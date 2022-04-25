using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookRepository.App.Domain;
using FluentAssertions;
using Xunit;

namespace BookRepository.Tests.EndToEnd
{
    public class GetBooksTests : IClassFixture<WebServerFixture>
    {
        private readonly WebServerFixture _fixture;
        private readonly HttpService<IEnumerable<Book>> _getBooksService;

        public GetBooksTests(WebServerFixture fixture)
        {
            _fixture = fixture;
            _getBooksService = new HttpService<IEnumerable<Book>>(_fixture.Client);
        }

        [Fact]
        public async Task GetAllBooks()
        {
            var books = await _getBooksService.GetData("/api/books");
            books.Should()
                .HaveCount(13);
        }

        [Fact]
        public async Task GetBooks_SortedById()
        {
            var books = await _getBooksService.GetData("/api/books/id");

            books.Should()
                .HaveCount(13)
                .And.BeInAscendingOrder(b => b.Id);
        }

        [Fact]
        public async Task GetBooks_ForId_B()
        {
            var books = await _getBooksService.GetData("/api/books/id/b");

            books.Should()
                .HaveCount(13)
                .And.AllSatisfy(book => book.PresentedId.Should().ContainEquivalentOf("b"))
                .And.BeInAscendingOrder(b => b.Id);
        }

        [Fact]
        public async Task GetBooks_ForId_b1()
        {
            var books = await _getBooksService.GetData("/api/books/id/b1");

            books.Should()
                .HaveCount(5)
                .And.AllSatisfy(book => book.PresentedId.Contains("b1", StringComparison.InvariantCultureIgnoreCase))
                .And.BeInAscendingOrder(b => b.Id);
        }

        [Fact]
        public async Task GetBooks_SortedByAuthor()
        {
            var books = await _getBooksService.GetData("/api/books/author/");

            books.Should()
                .HaveCount(13)
                .And.BeInAscendingOrder(b => b.Author);
        }

        [Fact]
        public async Task GetBooks_ForAuthor_Joe()
        {
            var books = await _getBooksService.GetData("/api/books/author/joe");

            books.Should()
                .ContainSingle(book => book.Author.Contains("joe", StringComparison.InvariantCultureIgnoreCase))
                .And.BeInAscendingOrder(b => b.Author);
        }

        [Fact]
        public async Task GetBooks_ForAuthor_Kut()
        {
            var books = await _getBooksService.GetData("/api/books/author/kut");

            books.Should()
                .ContainSingle(book => book.Author.Contains("kut", StringComparison.InvariantCultureIgnoreCase))
                .And.BeInAscendingOrder(b => b.Author);
        }

        [Fact]
        public async Task GetBooks_SortedByTitle()
        {
            var books = await _getBooksService.GetData("/api/books/title");

            books.Should()
                .HaveCount(13)
                .And.BeInAscendingOrder(b => b.Title);
        }

        [Fact]
        public async Task GetBooks_ForTitle_Deploy()
        {
            var books = await _getBooksService.GetData("/api/books/title/deploy");

            books.Should()
                .ContainSingle(book => book.Title.Contains("deploy", StringComparison.InvariantCultureIgnoreCase))
                .And.BeInAscendingOrder(b => b.Title);
        }

        [Fact]
        public async Task GetBooks_ForTitle_JRuby()
        {
            var books = await _getBooksService.GetData("/api/books/title/jruby");

            books.Should()
                .ContainSingle(book => book.Title.Contains("jruby", StringComparison.InvariantCultureIgnoreCase))
                .And.BeInAscendingOrder(b => b.Title);
        }

        [Fact]
        public async Task GetBooks_SortedByGenre()
        {
            var books = await _getBooksService.GetData("/api/books/genre");

            books.Should()
                .HaveCount(13)
                .And.BeInAscendingOrder(b => b.Genre);
        }

        [Fact]
        public async Task GetBooks_ForGenre_Deploy()
        {
            var books = await _getBooksService.GetData("/api/books/genre/deploy");

            books.Should().BeEmpty();
        }

        [Fact]
        public async Task GetBooks_ForGenre_Com()
        {
            var books = await _getBooksService.GetData("/api/books/genre/com");

            books.Should()
               .HaveCount(5)
               .And.AllSatisfy(book => book.Genre.Should().Contain("ter"))
               .And.BeInAscendingOrder(b => b.Genre);
        }

        [Fact]
        public async Task GetBooks_ForGenre_Ter()
        {
            var books = await _getBooksService.GetData("/api/books/genre/ter");

            books.Should()
                .HaveCount(5)
                .And.AllSatisfy(book => book.Genre.Should().Contain("ter"))
                .And.BeInAscendingOrder(b => b.Genre);
        }

        [Fact]
        public async Task GetBooks_SortedByPrice()
        {
            var books = await _getBooksService.GetData("/api/books/price");

            books.Should()
                .HaveCount(13)
                .And.BeInAscendingOrder(b => b.Price);
        }

        [Fact]
        public async Task GetBooks_ForPrice_Exact()
        {
            var books = await _getBooksService.GetData("/api/books/price/33.0");

            books.Should()
                .ContainSingle(book => book.Price == 33.0M);
        }

        [Fact]
        public async Task GetBooks_ForPrice_Range()
        {
            var books = await _getBooksService.GetData("/api/books/price/30.0&35.0");
            var p = new Book().Price >= 30.0M;
            books.Should()
                .HaveCount(2)
                .And.AllSatisfy(book =>
                {
                    book.Price.Should().BeInRange(30.0M, 35.0M);
                })
                .And.BeInAscendingOrder(book => book.Price);
        }

        [Fact]
        public async Task GetBooks_SortedByPublished()
        {
            var books = await _getBooksService.GetData("/api/books/published");

            books.Should()
                .HaveCount(13)
                .And.BeInAscendingOrder(b => b.Published);
        }

        [Fact]
        public async Task GetBooks_ForPublished_Year()
        {
            var books = await _getBooksService.GetData("/api/books/published/2012");

            books.Should()
                .HaveCount(2)
                .And.AllSatisfy(book =>
                {
                    book.Published.Should()
                        .BeOnOrAfter(new DateTime(2012, 1, 1))
                        .And.BeBefore(new DateTime(2013, 1, 1));
                })
                .And.BeInAscendingOrder(b => b.Published);
        }

        [Fact]
        public async Task GetBooks_ForPublished_YearMonth()
        {
            var books = await _getBooksService.GetData("/api/books/published/2012/8");


            books.Should()
                .ContainSingle()
                .And.AllSatisfy(book =>
                {
                    book.Published.Should()
                        .BeOnOrAfter(new DateTime(2012, 8, 1))
                        .And.BeBefore(new DateTime(2012, 9, 1));
                })
                .And.BeInAscendingOrder(b => b.Published);
        }

        [Fact]
        public async Task GetBooks_ForPublished_YearMonthDay()
        {
            var books = await _getBooksService.GetData("/api/books/published/2012/8/15");


            books.Should()
                .ContainSingle()
                .And.AllSatisfy(book =>
                {
                    book.Published.Should()
                        .BeOnOrAfter(new DateTime(2012, 8, 15))
                        .And.BeBefore(new DateTime(2012, 8, 16));
                })
                .And.BeInAscendingOrder(b => b.Published);
        }

        [Fact]
        public async Task GetBooks_SortedByDescription()
        {
            var books = await _getBooksService.GetData("/api/books/description");


            books.Should()
                .HaveCount(13)
                .And.BeInAscendingOrder(b => b.Description);
        }

        [Fact]
        public async Task GetBooks_ForDescription_Deploy()
        {
            var books = await _getBooksService.GetData("/api/books/description/deploy");


            books.Should()
                .HaveCount(2)
                .And.AllSatisfy(book =>
                {
                    book.Description.Contains("deploy", StringComparison.InvariantCultureIgnoreCase);
                })
                .And.BeInAscendingOrder(b => b.Description);
        }

        [Fact]
        public async Task GetBooks_ForDescription_Applications()
        {
            var books = await _getBooksService.GetData("/api/books/description/applications");


            books.Should()
                .ContainSingle(book => book.PresentedId == "B1")
                .And.AllSatisfy(book =>
                {
                    book.Description.Should().Contain("application");
                })
                .And.BeInAscendingOrder(b => b.Description);
        }
    }
}
