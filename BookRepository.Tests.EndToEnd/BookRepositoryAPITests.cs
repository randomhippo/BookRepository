using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookRepository.App.Domain;
using FluentAssertions;
using Xunit;

namespace BookRepository.Tests.EndToEnd
{
    public class BookRepositoryAPITests : IClassFixture<WebServerFixture>
    {
        private readonly WebServerFixture _fixture;

        public BookRepositoryAPITests(WebServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task GetAllBooks()
        {
            var service = new HttpService<IEnumerable<Book>>(_fixture.Client);
            var books = await service.GetData("/api/books");

            //TODO: Once database is reset for test run, this will return exactly 13 entities
            books.Should().HaveCountGreaterThanOrEqualTo(13);
        }

        [Fact]
        public async Task GetAllBooksSortedOnId()
        {
            var service = new HttpService<IEnumerable<Book>>(_fixture.Client);
            var books = await service.GetData("/api/books/id");

            //TODO: Once database is reset for test run, this will return exactly 13 entities
            books.Should().HaveCountGreaterThanOrEqualTo(13).And.BeInAscendingOrder(b => b.Id);
        }

        [Fact]
        public async Task GetAllBooksSortedFilterOnId1()
        {
            var service = new HttpService<IEnumerable<Book>>(_fixture.Client);
            var books = await service.GetData("/api/books/id/b");

            //TODO: Once database is reset for test run, this will return exactly 13 entities
            books.Should().HaveCountGreaterThanOrEqualTo(13).And.BeInAscendingOrder(b => b.Id);
        }

        [Fact]
        public async Task GetAllBooksSortedFilterOnId2()
        {
            var service = new HttpService<IEnumerable<Book>>(_fixture.Client);
            var books = await service.GetData("/api/books/id/b1");

            //TODO: Once database is reset for test run, this will return exactly 5 entities
            books.Should().HaveCountGreaterThanOrEqualTo(5).And.BeInAscendingOrder(b => b.Id);
        }

//GET https://host:port/api/books/author returns all sorted by author (B1-B13)
//GET https://host:port/api/books/author/joe returns all with author containing 'joe' sorted by author
//(B1)
//GET https://host:port/api/books/author/kut returns all with author containing 'kut' sorted by author
//(B1)
//GET https://host:port/api/books/title returns all sorted by title (B1-B13)
//GET https://host:port/api/books/title/deploy returns all with title containing 'deploy' sorted by title
//(B1)
//GET https://host:port/api/books/title/jruby returns all with title containing 'jruby' sorted by title (B1)
//GET https://host:port/api/books/genre returns all sorted by genre (B1-B13)
//GET https://host:port/api/books/genre/com returns all with genre containing 'com' sorted by genre
//(B1, B10-13)
//GET https://host:port/api/books/genre/ter returns all with genre containing 'ter' sorted by genre
//(B1, B10-13)
//GET https://host:port/api/books/price returns all sorted by price (B1-B13)
//GET https://host:port/api/books/price/33.0 returns all with price '33.0' (B1)
//GET https://host:port/api/books/price/30.0&35.0 returns all with price between '30.0' and '35.0'
//sorted by price(B1, B11)
//GET https://host:port/api/books/published returns all sorted by published_date (B1-B13)
//GET https://host:port/api/books/published/2012 returns all from '2012' sorted by published_date
//(B13, B1)
//GET https://host:port/api/books/published/2012/8 returns all from '2012-08' sorted by
//published_date(B1)
//GET https://host:port/api/books/published/2012/8/15 returns all from '2012-08-15' sorted by
//published_date(B1)
//GET https://host:port/api/books/description returns all sorted by description (B1-B13)
//GET https://host:port/api/books/description/deploy returns all with description containing 'deploy'
//sorted by description(B1, B13)
//GET https://host:port/api/books/description/applications returns all with description containing
//'applications' sorted by description(B1)
    }
}
