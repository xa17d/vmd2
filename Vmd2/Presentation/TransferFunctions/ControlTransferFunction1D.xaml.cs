using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Diagnostics;
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
using Vmd2.Processing.TransferFunctions;

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
            Vm.PropertyChanged += Vm_PropertyChanged;

            Vm.Items.Add(new TransferFunctionItem(0, Colors.Black));
            Vm.Items.Add(new TransferFunctionItem(1000, Colors.White));

            UpdateAll();
        }

        private Brush brushStrokeSelected = new SolidColorBrush(Colors.OrangeRed);
        private Brush brushStroke = new SolidColorBrush(Colors.Black);

        public event EventHandler TransferFunctionChanged;
        private void InvokeTransferFunctionChanged()
        {
            var e = TransferFunctionChanged;
            if (e!=null)
            {
                e(this, EventArgs.Empty);
            }
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
                    rectangle.MouseDown += Rectangle_MouseDown;
                    rectangle.MouseUp += Rectangle_MouseUp;
                    rectangle.MouseMove += Rectangle_MouseMove;
                    Update(tfItem);
                }
            }

            if (e.OldItems != null)
            {
                foreach (var item in e.OldItems)
                {
                    canvasTf.Children.Remove(((TransferFunctionItem)item).UiElement);
                }
            }

            InvokeTransferFunctionChanged();
        }

        private Point mouseDownPosition;

        private void Rectangle_MouseDown(object sender, MouseButtonEventArgs e)
        {
            var rectangle = (sender as Rectangle);
            if (rectangle != null)
            {
                Mouse.Capture(rectangle);
                mouseDownPosition = e.GetPosition(rectangle);
                Vm.CurrentItem = rectangle.DataContext as TransferFunctionItem;
            }
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(canvasTf);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double v = ((p.X - mouseDownPosition.X) / canvasTf.ActualWidth) * (Vm.MaxValue - Vm.MinValue) + Vm.MinValue;
                Vm.CurrentItem.Value = v;
            }
        }

        private void Rectangle_MouseUp(object sender, MouseButtonEventArgs e)
        {
            Mouse.Capture(null);
        }

        private void TfItem_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateAll(); // ((TransferFunctionItem)sender);
        }

        private void Vm_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            UpdateAll();
        }

        private void Update(TransferFunctionItem tfItem)
        {
            double x;

            x = (tfItem.Value - Vm.MinValue) / (Vm.MaxValue - Vm.MinValue) * canvasTf.ActualWidth;

            Canvas.SetLeft(tfItem.UiElement, x);
            if (tfItem == Vm.CurrentItem)
            {
                tfItem.UiElement.StrokeThickness = 2;
                tfItem.UiElement.Stroke = brushStrokeSelected;
            }
            else
            {
                tfItem.UiElement.StrokeThickness = 1;
                tfItem.UiElement.Stroke = brushStroke;
            }

            tfItem.UiElement.Fill = new SolidColorBrush(tfItem.Color);
        }

        private void UpdateAll()
        {
            foreach (var item in canvasTf.Children)
            {
                var rectangle = (item as Rectangle);
                if (rectangle != null)
                {
                    Update((TransferFunctionItem)rectangle.DataContext);
                }
            }

            InvokeTransferFunctionChanged();
        }

        private TransferFunction1DVm Vm { get { return (DataContext as TransferFunction1DVm); } }

        private void ButtonItemAdd_Click(object sender, RoutedEventArgs e)
        {
            double value = 0;
            if (Vm.Items.Count > 0)
            {
                value = Vm.Items[Vm.Items.Count - 1].Value + 100;
            }

            var item = new TransferFunctionItem(value);
            Vm.Items.Add(item);
            Vm.CurrentItem = item;
        }

        public TransferFunction1D CreateTransferFunction()
        {
            return Vm.CreateTransferFunction();
        }

        public void AddItem(double value, Color color)
        {
            Vm.Items.Add(new TransferFunctionItem(value, color));
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
