using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Vmd2.Processing
{
    class MarkerTest : Renderer
    {
        private int markerX;
        public int MarkerX
        {
            get { return markerX; }
            set { if (value != markerX) { markerX = value; OnPropertyChanged(); } }
        }

        private int markerY;
        public int MarkerY
        {
            get { return markerY; }
            set { if (value != markerY) { markerY = value; OnPropertyChanged(); } }
        }

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
        {
            if (MarkerX == x || MarkerY == y)
            {
                display.SetPixel(x, y, Colors.Orange);
            }
        }
    }
}
