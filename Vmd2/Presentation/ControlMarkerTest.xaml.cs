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
    /// Interaction logic for MarkerTest.xaml
    /// </summary>
    [ProcessingControl(typeof(MarkerTest))]
    public partial class ControlMarkerTest : UserControl
    {
        public ControlMarkerTest()
        {
            InitializeComponent();
        }

        private MarkerTest Element { get { return (MarkerTest)DataContext; } }

        private void buttonFromMarker_Click(object sender, RoutedEventArgs e)
        {
            Element.MarkerX = (int)Element.Display.Marker.X;
            Element.MarkerY = (int)Element.Display.Marker.Y;
        }
    }
}
