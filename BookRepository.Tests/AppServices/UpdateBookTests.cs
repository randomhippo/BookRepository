using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using BookRepository.App.AppServices;
using FluentValidation.TestHelper;
using Xunit;

namespace BookRepository.Tests.AppServices
{
    public class UpdateBookTests
    {
        public class ValidatorTests
        {

            [Fact]
            public void Given_ValidRequest_When_CheckingIfValid_Valid()
            {
                var validator = new UpdateBook.Validator();
                var request = new UpdateBook.Request
                {
                    Id = "B2",
                    UpdatedContent = new()
                    {
                        Author = "Author",
                        Description = "aaa",
                        Genre = "ger",
                        Price = "12.34",
                        Published = new DateTime(2022,1,1),
                        Title = "Title"
                    }

                };
                var result = validator.TestValidate(request);
                result.ShouldNotHaveValidationErrorFor("Price");
            }
        }
    }
}
