using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;

namespace Vmd2.Processing.TransferFunctions
{
    class TransferFunction1DItem : NotifyPropertyChanged
    {
        public TransferFunction1DItem(double value)
        {
            this.Value = value;
            this.a = 255;
        }

        public TransferFunction1DItem(double value, Color color)
        {
            this.Value = value;
            this.a = color.A;
            this.r = color.R;
            this.g = color.G;
            this.b = color.B;
        }

        private double val;
        public double Value { get { return val; } set { val = value; OnPropertyChanged(); } }

        private byte a;
        public byte A { get { return a; } set { a = value; OnPropertyChanged("Color"); } }

        private byte r;
        public byte R { get { return r; } set { r = value; OnPropertyChanged("Color"); } }

        private byte g;
        public byte G { get { return g; } set { g = value; OnPropertyChanged("Color"); } }

        private byte b;
        public byte B { get { return b; } set { b = value; OnPropertyChanged("Color"); } }

        public Color Color
        {
            get { return Color.FromArgb(a, r, g, b); }
            set
            {
                if (r != value.R || g != value.G || b != value.B)
                {
                    r = value.R;
                    g = value.G;
                    b = value.B;

                    OnPropertyChanged();
                }
            }
        }

        public object Tag { get; set; }

        public override string ToString()
        {
            return Value.ToString() + " - " + Color.ToString();
        }
    }
}
