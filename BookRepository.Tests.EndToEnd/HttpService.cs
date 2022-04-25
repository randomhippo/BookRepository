using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Text.Json;

namespace BookRepository.Tests.EndToEnd
{
    public class HttpService<T>
    {
        private readonly HttpClient _client;

        public HttpService(HttpClient client)
        {
            _client = client;
        }

        public async Task<T> GetData(string resourceString)
        {
            using (HttpResponseMessage response = await _client.GetAsync(resourceString, HttpCompletionOption.ResponseHeadersRead))
            {
                response.EnsureSuccessStatusCode();

                var stringResult = await response.Content.ReadAsStringAsync(CancellationToken.None);

                return JsonSerializer.Deserialize<T>(stringResult);
            }
        }
    }
}
