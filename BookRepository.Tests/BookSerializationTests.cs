using System;
using System.Text.Json;
using BookRepository.App.Converters;
using BookRepository.App.Domain;
using FluentAssertions;
using Xunit;
namespace BookRepository.Tests
{
    public class BookSerializationTests
    {
		[Fact]
        public void Given_JsonBook_When_Deserializing_BookIsReturned()
        {
            const string json = @"
	{
		""id"": ""B1"",
		""author"": ""Kutner, Joe"",
		""title"": ""Deploying with JRuby"",
		""genre"": ""Computer"",
		""price"": ""33.00"",
		""publish_date"": ""2012-08-15"",
		""description"": ""Deploying with JRuby is the missing link between enjoying JRuby and using it in the real world to build high-performance, scalable applications.""
	}";

			var expected = new Book
			{
				PresentedId = "B1",
				Author = "Kutner, Joe",
				Title = "Deploying with JRuby",
				Genre = "Computer",
				Price = 33.00M,
				Published = new DateTime(2012, 8, 15),
				Description = "Deploying with JRuby is the missing link between enjoying JRuby and using it in the real world to build high-performance, scalable applications."
			};

			var actual = JsonSerializer.Deserialize<Book>(json);
			actual.Should().Be(expected);
        }

		[Fact]
		public void Given_Book_When_Serializing_JsonIsReturned()
		{
			const string expected = @"{
  ""id"": ""B1"",
  ""author"": ""Kutner, Joe"",
  ""title"": ""Deploying with JRuby"",
  ""genre"": ""Computer"",
  ""price"": ""33.00"",
  ""publish_date"": ""2012-08-15"",
  ""description"": ""Deploying with JRuby is the missing link between enjoying JRuby and using it in the real world to build high-performance, scalable applications.""
}";

			var book = new Book
			{
				PresentedId = "B1",
				Author = "Kutner, Joe",
				Title = "Deploying with JRuby",
				Genre = "Computer",
				Price = 33.00M,
				Published = new DateTime(2012, 8, 15),
				Description = "Deploying with JRuby is the missing link between enjoying JRuby and using it in the real world to build high-performance, scalable applications."
			};

			var options = new JsonSerializerOptions()
			{
				WriteIndented = true				
			};
			options.Converters.Add(new CustomDateTimeConverter("yyyy-MM-dd"));

			var actual = JsonSerializer.Serialize<Book>(book, options);
			actual.Should().BeEquivalentTo(expected);
		}
	}
}
