using System.Diagnostics;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace MockGcc.UI.HttpClients
{
    public class State
    {
        public int MockAccountRate { get; set; }
        [JsonPropertyName("mockAccountLatency")]
        public int MockAccountLatency { get; set; }
        public int MockPersonInfoRate { get; set; }
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

        public async Task UpdateFrequency(State state)
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/main/setfrequency");

            request.Content = new StringContent(
                JsonSerializer.Serialize(state),
                System.Text.Encoding.UTF8,
                "application/json");

            await _httpClient.SendAsync(request);
        }

        public async Task<State> GetLatency()
        {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Post, "/api/main/getlatency");

            var response = await _httpClient.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();

            return JsonSerializer.Deserialize<State>(content);
        }
    }
}
