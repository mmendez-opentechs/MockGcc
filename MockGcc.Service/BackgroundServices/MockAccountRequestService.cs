using MockGcc.Service.HttpClients;

namespace MockGcc.Service.BackgroundServices
{
    public class MockAccountRequestService : BackgroundService
    {
        private readonly ILogger<MockAccountRequestService> _logger;
        private readonly MockAccountClient _mockAccountClient;
        private readonly State.State _state;

        public MockAccountRequestService(
            ILogger<MockAccountRequestService> logger,
            MockAccountClient mockAccountClient,
            State.State frequencyState)
        {
            _logger = logger;
            _mockAccountClient = mockAccountClient;
            _state = frequencyState;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Background service - MockAccount is called at: {time}", DateTimeOffset.Now);

                if (_state.TestVerticalAutoscaling)
                {
                    var latency = await _mockAccountClient.CallAccountInfo();

                    _state.SetMockAccountLatency(latency);

                    await Task.Delay((1000 / _state.MockAccountRequestRate), stoppingToken);
                }
                else
                {
                    await Task.Delay(1000, stoppingToken);
                }
            }
        }
    }
}
