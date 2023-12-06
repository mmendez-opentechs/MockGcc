using MockGcc.UI.HttpClients;

namespace MockGcc.UI.ViewModels
{
    public class MainViewModel : ViewModelBase, IMainViewModel
    {
        private readonly MockGccServiceClient _mockGccServiceClient;

        public MainViewModel(MockGccServiceClient mockGccServiceClient)
        {
            _mockGccServiceClient = mockGccServiceClient;
        }

        public int MockPersonInfoLatency { get; set; } = 82;
        public int MockAccountLatency { get; set; } = 73;

        public async Task UpdateFrequency(int mockPersonInfoRate, int mockAccountRate)
        {
            await _mockGccServiceClient.UpdateFrequency(new State()
            {
                MockPersonInfoRate = mockPersonInfoRate,
                MockAccountRate = mockAccountRate
            });
        }
    }
}
