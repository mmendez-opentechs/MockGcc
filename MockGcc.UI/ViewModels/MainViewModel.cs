using MockGcc.UI.HttpClients;
using PropertyChanged;

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
        [OnChangedMethod(nameof(OnStateChanged))]
        public int MockPersonInfoRequestRate { get; set; } = 10;
        [OnChangedMethod(nameof(OnStateChanged))]
        public int MockAccountRequestRate { get; set; } = 10;
        [OnChangedMethod(nameof(OnStateChanged))]
        public bool TestVerticalAutoscaling { get; set; } = false;
        public bool TestHorizontalAutoscaling { get; set; } = false;

        public void OnStateChanged()
        {
            _mockGccServiceClient.SetState(new State()
            {
                MockPersonInfoRequestRate = MockPersonInfoRequestRate,
                MockAccountRequestRate = MockAccountRequestRate,
                TestHorizontalAutoscaling = TestHorizontalAutoscaling,
                TestVerticalAutoscaling = TestVerticalAutoscaling
            });
        }
    }
}
