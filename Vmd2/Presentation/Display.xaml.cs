using System;
using System.Collections.Generic;
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

namespace Vmd2.Presentation
{
    /// <summary>
    /// Interaction logic for Display.xaml
    /// </summary>
    public partial class Display : UserControl, IDisplay
    {
        public Display()
        {
            InitializeComponent();
        }

        private DisplayImage display;

        public void Update()
        {
            if (display != null)
            {
                Dispatcher.Invoke(display.Update);
            }
        }

        public DisplayImage GetDisplay(int width, int height)
        {
            if (display == null || display.Width != width || display.Height != height)
            {
                Dispatcher.Invoke(
                    () =>
                    {
                        display = new DisplayImage(width, height);
                        image.Source = display.GetBitmap();
                    }
                );
            }

            return display;
        }

        public Point Marker { get; private set; }

        private void image_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            var p = e.GetPosition(image);
            Canvas.SetLeft(marker, p.X - marker.Width / 2);
            Canvas.SetTop(marker, p.Y - marker.Height / 2);

            Marker = p;
        }
    }
}
