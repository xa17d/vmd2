using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vmd2.Processing.TransferFunctions
{
    class TransferFunction1DBuilder : NotifyPropertyChanged
    {
        public TransferFunction1DBuilder()
        {
            MinValue = 0;
            MaxValue = 1100;
        }

        public TransferFunction1DBuilder Add(double value, Color color)
        {
            Items.Add(new TransferFunction1DItem(value, color));
            return this;
        }

        private ObservableCollection<TransferFunction1DItem> items = new ObservableCollection<TransferFunction1DItem>();
        public ObservableCollection<TransferFunction1DItem> Items { get { return items; } }

        private TransferFunction1DItem currentItem;
        public TransferFunction1DItem CurrentItem
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

        public static TransferFunction1DBuilder CreateTestBuilder()
        {
            var b = new TransferFunction1DBuilder();
            b.Items.Add(new TransferFunction1DItem(0, Colors.Black));
            b.Items.Add(new TransferFunction1DItem(1000, Colors.White));

            return b;
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
