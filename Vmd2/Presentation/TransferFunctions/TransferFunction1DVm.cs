using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Presentation.ViewModels;

namespace Vmd2.Presentation.TransferFunctions
{
    class TransferFunction1DVm : NotifyPropertyChanged
    {
        private ObservableCollection<TransferFunctionItem> items = new ObservableCollection<TransferFunctionItem>();
        public ObservableCollection<TransferFunctionItem> Items { get { return items; } }

        private TransferFunctionItem currentItem;
        public TransferFunctionItem CurrentItem
        {
            get { return currentItem; }
            set
            {
                if (value != currentItem)
                {
                    currentItem = value;
                    OnPropertyChanged();
                }
            }
        }

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
