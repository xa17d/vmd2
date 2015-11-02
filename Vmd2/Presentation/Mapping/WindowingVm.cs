using System.Collections.ObjectModel;
using Vmd2.Presentation.ViewModels;

namespace Vmd2.Presentation.Mapping
{
    class WindowingVm : NotifyPropertyChanged
    {
        public WindowingVm()
        {
            MinValue = 0;
            MaxValue = 1100;
        }

        private ObservableCollection<WindowingItem> items = new ObservableCollection<WindowingItem>();
        public ObservableCollection<WindowingItem> Items { get { return items; } }

        private double minValue;
        public double MinValue
        {
            get { return minValue; }
            set
            {
                if (minValue != value)
                {
                    minValue = value;
                    OnPropertyChanged();
                }
            }
        }

        private double maxValue;
        public double MaxValue
        {
            get { return maxValue; }
            set
            {
                if (maxValue != value)
                {
                    maxValue = value;
                    OnPropertyChanged();
                }
            }
        }
    }
}
