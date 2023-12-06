using System.ComponentModel;

namespace MockGcc.UI.ViewModels
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        public int MockAccountLatency { get; set; }
        public int MockPersonInfoLatency { get; set; }
        public Task UpdateFrequency(int mockPersonInfoRate, int mockAccountRate);
    }
}
