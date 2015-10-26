using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using Vmd2.Presentation.ViewModels;

namespace Vmd2.Presentation.TransferFunctions
{
    /// <summary>
    /// Interaction logic for ControlTransferFunction1D.xaml
    /// </summary>
    public partial class ControlTransferFunction1D : UserControl
    {
        public ControlTransferFunction1D()
        {
            InitializeComponent();
            DataContext = new TransferFunction1DVm();

            Vm.Items.CollectionChanged += Items_CollectionChanged;
            
        }

        private void Items_CollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null)
            {
                foreach (var item in e.NewItems)
                {
                    var rectangle = new Rectangle()
                    {
                        StrokeThickness = 1,
                        Stroke = new SolidColorBrush(Colors.Black),
                        DataContext = item,
                        Width = 8,
                        Height = canvasTf.Height
                    };

                    Canvas.SetLeft(rectangle, 0);
                    Canvas.SetTop(rectangle, 0);

                    canvasTf.Children.Add(rectangle);

                    var tfItem = (TransferFunctionItem)item;
                    tfItem.UiElement = rectangle;
                    tfItem.PropertyChanged += TfItem_PropertyChanged;
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    canvasTf.Children.Remove(((TransferFunctionItem)item).UiElement);
                }
            }
        }

        private void TfItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            Update((TransferFunctionItem)sender);
        }

        private void Update(TransferFunctionItem tfItem)
        {
            Canvas.SetLeft(tfItem.UiElement, tfItem.Value);
            tfItem.UiElement.Fill = new SolidColorBrush(tfItem.Color);
        }

        private void UpdateAll()
        {
            foreach (var item in canvasTf.Children)
            {
                var rectangle = (Rectangle)item;
                Update((TransferFunctionItem)rectangle.DataContext);
            }
        }

        private TransferFunction1DVm Vm { get { return (DataContext as TransferFunction1DVm); } }

        private void ButtonItemAdd_Click(object sender, RoutedEventArgs e)
        {
            var item = new TransferFunctionItem(100);
            Vm.Items.Add(item);
            Vm.CurrentItem = item;
        }

        private void ButtonItemRemove_Click(object sender, RoutedEventArgs e)
        {
            var item = Vm.CurrentItem;
            if (item != null)
            {
                Vm.CurrentItem = null;
                Vm.Items.Remove(item);
            }
        }
    }
}
