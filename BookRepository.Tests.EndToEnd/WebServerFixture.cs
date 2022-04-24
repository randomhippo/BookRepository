using Microsoft.AspNetCore.Mvc.Testing;
using System;
using System.Net.Http;

namespace BookRepository.Tests.EndToEnd
{
    public class WebServerFixture : IDisposable
    {
        WebApplicationFactory<Program> _application;


        public WebServerFixture()
        {
            _application = new WebApplicationFactory<Program>()
                                .WithWebHostBuilder(builder => { builder.UseSetting("databaseName", DatabaseName); });

            Client = _application.CreateClient();

        }

        public HttpClient Client { get; private set; }

        public virtual string DatabaseName { get; private set; } = "TestBooks.db";

        public void Dispose()
        {
            _application.Dispose();
            Client.Dispose();
        }
    }


}
