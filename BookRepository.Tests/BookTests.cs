using BookRepository.App.Domain;
using System;
using Xunit;
using FluentAssertions;

namespace BookRepository.Tests
{
    public class BookTests
    {
        [Fact]
        public void Given_TwoEqualBooks_When_CheckingEquals_Then_Are_Equal()
        {
            var book1 = new Book()
            {
                Author = "A",
                Description = "B",
                Genre = "C",
                PresentedId = "I1",
                Price = 12.3M,
                Published = new DateTime(2022, 1, 1),
                Title = "T"
            };

            var book2 = new Book()
            {
                Author = "A",
                Description = "B",
                Genre = "C",
                PresentedId = "I1",
                Price = 12.3M,
                Published = new DateTime(2022, 1, 1),
                Title = "T"
            };

            book1.Should().Be(book2);
        }

        [Fact]
        public void Given_TwoDifferentBooks_When_CheckingEquals_Then_Are_Equal()
        {
            var book1 = new Book()
            {
                Author = "Aaaaaaaa",
                Description = "B",
                Genre = "C",
                PresentedId = "I1",
                Price = 12.3M,
                Published = new DateTime(2022, 1, 1),
                Title = "T"
            };

            var book2 = new Book()
            {
                Author = "A",
                Description = "B",
                Genre = "C",
                PresentedId = "I1",
                Price = 12.3M,
                Published = new DateTime(2022, 1, 1),
                Title = "T"
            };

            book1.Should().NotBe(book2);
        }
    }
}
