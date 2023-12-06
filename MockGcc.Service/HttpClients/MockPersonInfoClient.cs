using System.Diagnostics;

namespace MockGcc.Service.HttpClients
{
    public class MockPersonInfoClient : HttpClient
    {
        private HttpClient _httpClient;

        public MockPersonInfoClient(HttpClient httpClient) {
            _httpClient = httpClient;
        }

        /// <summary>
        /// Returns time elapsed in milliseconds
        /// </summary>
        /// <returns></returns>
        public async Task<int> CallPersonInfo()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, "api/personinfo");

            var stopwatch = Stopwatch.StartNew();

            await _httpClient.SendAsync(request);

            stopwatch.Stop();

            return (int)stopwatch.ElapsedMilliseconds;
        }
    }
}
