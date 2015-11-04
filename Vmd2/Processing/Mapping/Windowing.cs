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
            set
            {
                if (value != windowCenter)
                {
                    windowCenter = value;
                    OnPropertyChanged();

                    MaxWindowWidth = Math.Min(
                        Math.Abs(MaxIntensity - WindowCenter), 
                        Math.Abs(WindowCenter - MinIntensity)
                    );

                    if (WindowWidth > MaxWindowWidth) { WindowWidth = Math.Max(1, MaxWindowWidth); }
                }
            }
        }

        private double windowWidth;
        public double WindowWidth
        {
            get { return windowWidth; }
            set { if (value != windowWidth) { windowWidth = value; OnPropertyChanged(); } }
        }

        private double maxIntensity;
        public double MaxIntensity
        {
            get { return maxIntensity; }
            set { if (value != maxIntensity) { maxIntensity = value; OnPropertyChanged(); } }
        }

        private double minIntensity;
        public double MinIntensity
        {
            get { return minIntensity; }
            set { if (value != minIntensity) { minIntensity = value; OnPropertyChanged(); } }
        }

        private double maxWindowWidth;
        public double MaxWindowWidth
        {
            get { return maxWindowWidth; }
            private set { if (value != maxWindowWidth) { maxWindowWidth = value; OnPropertyChanged(); } }
        }

        public Color GetColor(double value)
        {
            // see http://www.codeproject.com/Articles/466955/Medical-image-visualization-using-WPF
            byte rgb;
            if (value < (WindowCenter - WindowWidth / 2))
            {
                //black
                rgb = 0x00;
            }
            else if (value > (WindowCenter + WindowWidth / 2))
            {
                //white
                rgb = 0xFF;
            }
            else
            {
                //grey
                double v = (value - (WindowCenter - WindowWidth / 2)) / WindowWidth * 0xFF;
                //if (v>255) { v = 255; }
                //else if (v<0) { v = 0; }
                rgb = Convert.ToByte(v);
            }

            return Color.FromArgb(0xFF, rgb, rgb, rgb);
        }
    }
}
