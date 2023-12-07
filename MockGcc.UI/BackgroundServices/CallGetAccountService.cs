using Microsoft.Extensions.Hosting;
using MockGcc.UI.HttpClients;
using MockGcc.UI.ViewModels;

namespace MockGcc.UI.BackgroundServices
{
    public class CallGetAccountService : BackgroundService
    {
        private readonly ILogger<CallGetAccountService> _logger;
        private readonly MockGccServiceClient _client;
        private readonly IMainViewModel _mainViewModel;

        private DateTime _startTime;

        private bool _keepRunning = true;

        public CallGetAccountService(
            ILogger<CallGetAccountService> logger,
            MockGccServiceClient client,
            IMainViewModel mainViewModel)
        {
            _logger = logger;
            _client = client;
            _mainViewModel = mainViewModel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(CallGetAccountService)} running.");

            try
            {
                _startTime = DateTime.Now;
                while (_keepRunning)
                {
                    if (_mainViewModel.TestHorizontalAutoscaling)
                    {
                        await DoWork();
                        await Task.Delay((1000 / _mainViewModel.MockAccountRequestRate), stoppingToken);
                    }
                    else
                    {
                        await Task.Delay(1000, stoppingToken);
                    }
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"{nameof(CallGetAccountService)} is stopping.");
            }
        }

        private async Task DoWork()
        {
            try
            {
                var latency = await _client.CallAccount(); // returns latency

                _mainViewModel.MockPersonInfoLatency = latency;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }

            _logger.LogInformation(
                $"{nameof(CallGetAccountService)} is working");
        }
    }
}
