using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockGcc.UI.HttpClients
{
    public class State
    {
        public bool TestVerticalAutoscaling { get; set; } = false;
        public bool TestHorizontalAutoscaling { get; set; } = false;
        public int MockAccountRequestRate { get; set; }
        [JsonPropertyName("mockAccountLatency")]
        public int MockAccountLatency { get; set; }
        public int MockPersonInfoRequestRate { get; set; }
        [JsonPropertyName("mockPersonInfoLatency")]
        public int MockPersonInfoLatency { get; set; }
    }

    public class MockGccServiceClient : HttpClient
    {
        private HttpClient _httpClient;

        public MockGccServiceClient(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task SetState(State state)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/main/setstate");

            request.Content = new StringContent(
                JsonSerializer.Serialize(state),
                System.Text.Encoding.UTF8,
                "application/json");

            await _httpClient.SendAsync(request);
        }

        public async Task<State?> GetLatency()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/main/getlatency");

            var response = await _httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<State>(content);
        }

        public async Task<int> CallPersonInfo()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/main/callpersoninfo");

            var stopwatch = Stopwatch.StartNew();

            var response = await _httpClient.SendAsync(request);

            stopwatch.Stop();

            return (int)stopwatch.ElapsedMilliseconds;
        }

        public async Task<int> CallAccount()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/main/callaccount");

            var stopwatch = Stopwatch.StartNew();

            var response = await _httpClient.SendAsync(request);

            stopwatch.Stop();

            return (int)stopwatch.ElapsedMilliseconds;
        }
    }
}
