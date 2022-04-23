using System.Threading.Tasks;
using Xunit;

namespace BookRepository.Tests.EndToEnd
{
    public class WebServerFixtureTests : IClassFixture<WebServerFixture>
    {
        private readonly WebServerFixture _fixture;

        public WebServerFixtureTests(WebServerFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task TestFixtureWorks()
        {
            var result = await _fixture.Client.GetAsync("/WeatherForecast");

            Assert.NotNull(result);
        }
    }
}