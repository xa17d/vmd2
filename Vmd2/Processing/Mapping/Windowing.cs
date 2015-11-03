using System;
using System.Windows.Media;

namespace Vmd2.Processing.Mapping
{
    public class Windowing : NotifyPropertyChanged
    {
        public Windowing()
        {
            // default values
            WindowCenter = 500;
            WindowWidth = 300;
        }

        private double windowCenter;
        public double WindowCenter
        {
            get { return windowCenter; }
            set { if (value != windowCenter) { windowCenter = value; OnPropertyChanged(); } }
        }

        private double windowWidth;
        public double WindowWidth
        {
            get { return windowWidth; }
            set { if (value != windowWidth) { windowWidth = value; OnPropertyChanged(); } }
        }

        public Color GetColor(double value)
        {
            // see http://www.codeproject.com/Articles/466955/Medical-image-visualization-using-WPF
            byte rgb;
            if (value < WindowCenter - WindowWidth / 2)
            {
                //black
                rgb = 0x00;
            }
            else if (value > WindowCenter + WindowWidth / 2)
            {
                //white
                rgb = 0xFF;
            }
            else
            {
                //grey
                rgb = Convert.ToByte((value - (WindowCenter - WindowWidth / 2)) / WindowWidth * 0xFF);
            }

            return Color.FromArgb(0xFF, rgb, rgb, rgb);
        }
    }
}
