using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Presentation.TransferFunctions
{
    class TransferFunction1DVm : NotifyPropertyChanged
    {
        public TransferFunction1DVm()
        {
            MinValue = 0;
            MaxValue = 1100;
        }

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

        public TransferFunction1D CreateTransferFunction()
        {
            var tf = new TransferFunction1D();

            var sortedItems = from i in Items orderby i.Value select i;

            foreach (var item in sortedItems)
            {
                tf.Add(item.Value, item.Color);
            }

            return tf;
        }
    }
}
