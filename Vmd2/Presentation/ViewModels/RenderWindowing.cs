using Vmd2.Processing.Mapping;

namespace Vmd2.Presentation.ViewModels
{
    class RenderWindowing : RenderVm
    {
        private int slice;
        public int Slice
        {
            get { return slice; }
            set
            {
                if (value != slice)
                {
                    slice = value;
                    ((WindowingRenderer)Renderer).Slice = value;

                    OnPropertyChanged();
                    Render();
                }
            }
        }

        private int center = 500;
        public int WindowCenter { get { return center; } set { center = value; ((WindowingRenderer)Renderer).Window.WindowCenter = center; OnPropertyChanged(); /*calculateMinMax();*/ } }
        private int width = 300;
        public int WindowWidth { get { return width; } set { width = value; ((WindowingRenderer)Renderer).Window.WindowWidth = width; OnPropertyChanged(); /*calculateMinMax();*/ } }

        /*
        private double min = 0;
        private double max = 1100;
        public double WindowMin
        {
            set
            {
                min = value;
                calculateMinMax();
            }
        }
        public double WindowMax
        {
            set
            {
                max = value;
                calculateMinMax();
            }
        }

        private void calculateMinMax()
        {
            sliderCenter.Minimum = min + 1;
            sliderCenter.Maximum = max - 1;

            sliderWidth.Minimum = 1;

            double lower = Math.Abs(WindowCenter - min);
            double upper = Math.Abs(WindowCenter - max);
            sliderWidth.Maximum = Math.Min(lower, upper);
        }
    */
    }
}
