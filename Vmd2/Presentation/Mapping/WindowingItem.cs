using Vmd2.Presentation.ViewModels;

namespace Vmd2.Presentation.Mapping
{
    class WindowingItem : NotifyPropertyChanged
    {
        public WindowingItem()
        {
            this.Center = 500;
            this.Width = 300;
        }

        public WindowingItem(double center, double width)
        {
            this.Center = center;
            this.Width = width;
        }

        private double center;
        public double Center { get { return center; } set { center = value; OnPropertyChanged(); } }

        private double width;
        public double Width { get { return width; } set { width = value; OnPropertyChanged(); } }

        public override string ToString()
        {
            return Center.ToString() + " +/- " + Width.ToString();
        }
    }
}
