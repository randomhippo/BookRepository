using BookRepository.App.AppServices;
using FluentValidation.TestHelper;
using Xunit;

namespace BookRepository.Tests.AppServices
{
    /// <summary>
    /// Sample tests for a single app service.
    /// Intended to show how parts can be tested in isolation.
    /// </summary>
    public class GetBooksPublishedInIntervalTests
    {
        /// <summary>
        /// Handler tests should go here. In this case a fake db should be injected into the handler, and changes to dbset should be made
        /// </summary>
        public class HandlerTests 
        {

        }

        /// <summary>
        /// Used to perform tests on validation part of pipeline.
        /// </summary>
        public class ValidatorTests
        {

            [Fact]
            public void Given_RequestForYear10000_When_CheckingIfValid_NotValid()
            {
                //Note: if running multiple tests, this should be in a test fixture
                var validator = new GetBooksPublishedInInterval.Validator();
                var request = new GetBooksPublishedInInterval.Request
                {
                    Year = 10000
                };
                var result = validator.TestValidate(request);
                result.ShouldHaveValidationErrorFor("Year");
            }

            [Fact]
            public void Given_RequestForYear9000_When_CheckingIfValid_Valid()
            {
                //Note: if running multiple tests, this should be in a test fixture
                var validator = new GetBooksPublishedInInterval.Validator();
                var request = new GetBooksPublishedInInterval.Request
                {
                    Year = 9000
                };
                var result = validator.TestValidate(request);
                result.ShouldNotHaveValidationErrorFor("Year");
            }
        }
    }
}
