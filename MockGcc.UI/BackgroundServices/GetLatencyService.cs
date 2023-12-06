using Microsoft.Extensions.Hosting;
using MockGcc.UI.HttpClients;
using MockGcc.UI.ViewModels;

namespace MockGcc.UI.BackgroundServices
{
    public class GetLatencyService : BackgroundService
    {
        private readonly ILogger<GetLatencyService> _logger;
        private readonly MockGccServiceClient _client;
        private readonly IMainViewModel _mainViewModel;

        private DateTime _startTime;

        private bool _keepRunning = true;

        public GetLatencyService(
            ILogger<GetLatencyService> logger,
            MockGccServiceClient client,
            IMainViewModel mainViewModel)
        {
            _logger = logger;
            _client = client;
            _mainViewModel = mainViewModel;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation($"{nameof(GetLatencyService)} running.");

            using PeriodicTimer timer = new(TimeSpan.FromSeconds(1));

            try
            {
                _startTime = DateTime.Now;
                while (await timer.WaitForNextTickAsync(stoppingToken) && _keepRunning)
                {
                    await DoWork();
                }
            }
            catch (OperationCanceledException)
            {
                _logger.LogInformation($"{nameof(GetLatencyService)} is stopping.");
            }
        }

        private async Task DoWork()
        {
            try
            {
                var _state = await _client.GetLatency();

                _mainViewModel.MockPersonInfoLatency = _state.MockPersonInfoLatency;
                _mainViewModel.MockAccountLatency = _state.MockAccountLatency;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception.Message);
            }

            _logger.LogInformation(
                $"{nameof(GetLatencyService)} is working");
        }
    }
}
