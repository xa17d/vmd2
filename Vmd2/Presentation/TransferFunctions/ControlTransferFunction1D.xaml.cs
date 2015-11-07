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
using Vmd2.Processing;
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

            displayImage = new DisplayImage(500, 100);
            imageTfBackground.Source = displayImage.GetBitmap();

            DataContextChanged += OnDataContextChanged;
            DataContext = TransferFunction1DBuilder.CreateTestBuilder();

            Loaded += OnLoaded;
        }

        protected override void OnRenderSizeChanged(SizeChangedInfo sizeInfo)
        {
            base.OnRenderSizeChanged(sizeInfo);

            UpdateAll();
        }

        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            UpdateAll();
        }

        private void OnDataContextChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            if (e.OldValue != null)
            {
                var b = (TransferFunction1DBuilder)e.OldValue;
                b.Items.CollectionChanged -= Items_CollectionChanged;
                b.PropertyChanged -= Vm_PropertyChanged;
            }

            if (e.NewValue != null)
            {
                var b = (TransferFunction1DBuilder)e.NewValue;
                b.Items.CollectionChanged += Items_CollectionChanged;
                b.PropertyChanged += Vm_PropertyChanged;

                Items_CollectionChanged(null, new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Add, b.Items));
                UpdateAll();
            }
        }

        private DisplayImage displayImage;
        public void UpdateImage(Image3D image)
        {
            Histogram histogram = new Histogram(image, displayImage.Width);

            TransferFunction1D tf = null;

            Dispatcher.Invoke(
                    () =>
                    {
                        tf = CreateTransferFunction();
                    }
                );

            histogram.Render(displayImage, tf);

            Dispatcher.Invoke(
                () =>
                {
                    displayImage.Update();

                    imageTfBackground.Width = canvasTf.ActualWidth;
                    imageTfBackground.Height = canvasTf.ActualHeight;
                    //UpdateAll();
                }
            );
        }

        private Brush brushStrokeSelected = new SolidColorBrush(Colors.OrangeRed);
        private Brush brushStroke = new SolidColorBrush(Colors.Gray);

        public event EventHandler TransferFunctionChanged;
        private void InvokeTransferFunctionChanged()
        {
            var e = TransferFunctionChanged;
            if (e != null)
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
                        Height = 20 // canvasTf.Height
                    };

                    Canvas.SetLeft(rectangle, 0);
                    Canvas.SetTop(rectangle, 0);

                    canvasTf.Children.Add(rectangle);

                    var tfItem = (TransferFunction1DItem)item;
                    tfItem.Tag = rectangle;
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
                    canvasTf.Children.Remove((UIElement)((TransferFunction1DItem)item).Tag);
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
                Builder.CurrentItem = rectangle.DataContext as TransferFunction1DItem;
            }
        }

        private void Rectangle_MouseMove(object sender, MouseEventArgs e)
        {
            var p = e.GetPosition(canvasTf);

            if (e.LeftButton == MouseButtonState.Pressed)
            {
                double v = ((p.X - mouseDownPosition.X) / canvasTf.ActualWidth) * (Builder.MaxValue - Builder.MinValue) + Builder.MinValue;
                Builder.CurrentItem.Value = v;
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

        private void Update(TransferFunction1DItem tfItem)
        {
            double x;

            x = (tfItem.Value - Builder.MinValue) / (Builder.MaxValue - Builder.MinValue) * canvasTf.ActualWidth;

            var rectangle = (Rectangle)tfItem.Tag;
            Canvas.SetLeft(rectangle, x);
            if (tfItem == Builder.CurrentItem)
            {
                rectangle.StrokeThickness = 2;
                rectangle.Stroke = brushStrokeSelected;
            }
            else
            {
                rectangle.StrokeThickness = 1;
                rectangle.Stroke = brushStroke;
            }

            rectangle.Fill = new SolidColorBrush(tfItem.Color);
        }

        private void UpdateAll()
        {
            foreach (var item in canvasTf.Children)
            {
                var rectangle = (item as Rectangle);
                if (rectangle != null)
                {
                    Update((TransferFunction1DItem)rectangle.DataContext);
                }
            }

            InvokeTransferFunctionChanged();
        }

        private TransferFunction1DBuilder Builder { get { return (DataContext as TransferFunction1DBuilder); } }

        private void ButtonItemAdd_Click(object sender, RoutedEventArgs e)
        {
            double value = 0;
            if (Builder.Items.Count > 0)
            {
                value = Builder.Items[Builder.Items.Count - 1].Value + 100;
            }

            var item = new TransferFunction1DItem(value);
            Builder.Items.Add(item);
            Builder.CurrentItem = item;
        }

        public TransferFunction1D CreateTransferFunction()
        {
            return Builder.CreateTransferFunction();
        }

        public void AddItem(double value, Color color)
        {
            Builder.Items.Add(new TransferFunction1DItem(value, color));
        }

        private void ButtonItemRemove_Click(object sender, RoutedEventArgs e)
        {
            var item = Builder.CurrentItem;
            if (item != null)
            {
                Builder.CurrentItem = null;
                Builder.Items.Remove(item);
            }
        }

        private void imageTfBackground_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            if (e.ClickCount == 2) // double click
            {
                var x = e.GetPosition(canvasTf).X;
                var value = x * (Builder.MaxValue - Builder.MinValue) / canvasTf.ActualWidth + Builder.MinValue;

                var item = new TransferFunction1DItem(value, CreateTransferFunction().GetColor(value));
                Builder.Items.Add(item);
                Builder.CurrentItem = item;
            }
        }
    }
}
