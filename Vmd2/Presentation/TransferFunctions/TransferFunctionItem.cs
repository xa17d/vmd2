using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Shapes;

namespace Vmd2.Presentation.TransferFunctions
{
    class TransferFunctionItem : NotifyPropertyChanged
    {
        public TransferFunctionItem(double value)
        {
            this.Value = value;
            this.a = 255;
        }

        public TransferFunctionItem(double value, Color color)
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
        public byte A { get { return a; } set { a = value; OnPropertyChanged(); OnPropertyChanged("Color"); } }

        private byte r;
        public byte R { get { return r; } set { r = value; OnPropertyChanged(); OnPropertyChanged("Color"); } }

        private byte g;
        public byte G { get { return g; } set { g = value; OnPropertyChanged(); OnPropertyChanged("Color"); } }

        private byte b;
        public byte B { get { return b; } set { b = value; OnPropertyChanged(); OnPropertyChanged("Color"); } }

        public Color Color
        {
            get { return Color.FromArgb(a, r, g, b); }
        }

        public Rectangle UiElement { get; set; }

        public override string ToString()
        {
            return Value.ToString() + " - " + Color.ToString();
        }
    }
}
