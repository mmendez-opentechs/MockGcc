using System.ComponentModel;

namespace MockGcc.UI.ViewModels
{
    public interface IMainViewModel : INotifyPropertyChanged
    {
        public int MockAccountLatency { get; set; }
        public int MockPersonInfoLatency { get; set; }
        public int MockAccountRequestRate { get; set; }
        public int MockPersonInfoRequestRate { get; set; }
        public bool TestVerticalAutoscaling { get; set; }
        public bool TestHorizontalAutoscaling { get; set; }
        public void OnStateChanged();
    }
}
