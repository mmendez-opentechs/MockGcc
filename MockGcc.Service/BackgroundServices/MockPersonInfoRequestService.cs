using MockGcc.Service.HttpClients;

namespace MockGcc.Service.BackgroundServices
{
    public class MockPersonInfoRequestService : BackgroundService
    {
        private readonly ILogger<MockPersonInfoRequestService> _logger;
        private readonly MockPersonInfoClient _mockPersonInfoClient;
        private readonly State.State _state;

        public MockPersonInfoRequestService(
            ILogger<MockPersonInfoRequestService> logger,
            MockPersonInfoClient mockPersonInfoClient,
            State.State state)
        {
            _logger = logger;
            _mockPersonInfoClient = mockPersonInfoClient;
            _state = state;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background service - MockPersonInfo is called at: {time}", DateTimeOffset.Now);

                if (_state.TestVerticalAutoscaling)
                {
                    var latency = await _mockPersonInfoClient.CallPersonInfo();

                    _state.SetMockPersonInfoLatency(latency);
                    
                    await Task.Delay((1000 / _state.MockPersonInfoRequestRate), stoppingToken);
                }
                else
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
