using System.Diagnostics;
using System.Net.Http;

namespace MockGcc.Service.HttpClients
{
    public class MockAccountClient : HttpClient
    {
        private HttpClient _httpClient;

        public MockAccountClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }
        /// <summary>
        /// Returns time elapsed in milliseconds
        /// </summary>
        /// <returns></returns>
        public async Task<(int, object)> CallAccountInfo()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/accountinfo");

            var stopwatch = Stopwatch.StartNew();

            var response = await _httpClient.SendAsync(request);

            var responseContent = await response.Content.ReadAsStringAsync();

            stopwatch.Stop();

            return ((int)stopwatch.ElapsedMilliseconds, responseContent);
        }
    }
}
