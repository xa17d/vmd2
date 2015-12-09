using System.Windows;
using System.Windows.Controls;
using Vmd2.Processing.Segmentation;

namespace Vmd2.Presentation.Segmentation
{
    /// <summary>
    /// Interaction logic for ControlRegionGrowing.xaml
    /// </summary>
    [ProcessingControl(typeof(RegionGrowing))]
    public partial class ControlRegionGrowing : UserControl
    {
        public ControlRegionGrowing()
        {
            InitializeComponent();
        }

        private RegionGrowing Element { get { return (RegionGrowing)DataContext; } }

        private void buttonFromMarker_Click(object sender, RoutedEventArgs e)
        {
            Element.MarkerX = (int)Element.Display.Marker.X;
            Element.MarkerY = (int)Element.Display.Marker.Y;
            //TODO
            //Element.MarkerZ = (int)Element.Display.Marker.Z;
        }
    }
}
