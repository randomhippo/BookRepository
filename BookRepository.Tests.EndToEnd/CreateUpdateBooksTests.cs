using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace BookRepository.Tests.EndToEnd
{
    public class CreateUpdateBooksTests : IClassFixture<ReadWriteFixture>
    {
        private readonly ReadWriteFixture _fixture;

        public CreateUpdateBooksTests(ReadWriteFixture fixture)
        {
            _fixture = fixture;
        }

        [Fact]
        public async Task CreateUpdateBooksAsync()
        {
            
            
        }
    }
}
