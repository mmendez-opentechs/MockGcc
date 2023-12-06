using MockGcc.Service.HttpClients;

namespace MockGcc.Service.BackgroundServices
{
    public class MockServiceRequester : BackgroundService
    {
        private readonly ILogger<MockServiceRequester> _logger;
        private readonly MockPersonInfoClient _mockPersonInfoClient;
        private readonly MockAccountClient _mockAccountClient;
        private readonly State.State _frequencyState;

        public MockServiceRequester(
            ILogger<MockServiceRequester> logger,
            MockPersonInfoClient mockPersonInfoClient,
            MockAccountClient mockAccountClient,
            State.State frequencyState)
        {
            _logger = logger;
            _mockPersonInfoClient = mockPersonInfoClient;
            _mockAccountClient = mockAccountClient;
            _frequencyState = frequencyState;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            // Open thread with while loop per required rate
            ThreadPool.QueueUserWorkItem(async (t) =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Background service - MockPersonInfo is called at: {time}", DateTimeOffset.Now);

                    // make call but don't wait for it
                    var personInfoLatency = _mockPersonInfoClient.CallPersonInfo();

                    // set latency when call is finished
                    personInfoLatency.ContinueWith((t) =>
                    {
                        var latency = t.Result;

                        _frequencyState.SetMockPersonInfoLatency(latency);
                    });

                    await Task.Delay((1000 / _frequencyState.MockPersonInfoRate), stoppingToken);
                }
            });

            // Open thread with while loop per required rate
            ThreadPool.QueueUserWorkItem(async (t) =>
            {
                while (!stoppingToken.IsCancellationRequested)
                {
                    _logger.LogInformation("Background service - MockAccount is called at: {time}", DateTimeOffset.Now);

                    // make call but don't wait for it
                    var accountLatency = _mockAccountClient.CallAccountInfo();

                    // set latency when call is finished
                    accountLatency.ContinueWith((t) =>
                    {
                        var latency = t.Result;

                        _frequencyState.SetMockAccountLatency(latency);
                    });

                    await Task.Delay((1000 / _frequencyState.MockAccountRate), stoppingToken);
                }
            });
        }
    }
}
