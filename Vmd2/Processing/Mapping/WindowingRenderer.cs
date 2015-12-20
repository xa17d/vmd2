using System;

namespace Vmd2.Processing.Mapping
{
    class WindowingRenderer : Renderer
    {
        public WindowingRenderer()
        {
            this.Window = new Windowing();
            this.Window.PropertyChanged += Window_PropertyChanged;
        }

        private void Window_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Window");
        }
        
        public Windowing Window { private set; get; }
        public double WindowCenter { set { Window.WindowCenter = value; } }
        public double WindowWidth { set { Window.WindowWidth = value; } }

        protected override void OnValidate(Image3D image)
        {
            if (image.LengthZ != 1) { throw new LogException("can only render Images with exactly one slice"); }

            Window.MaxIntensity = image.Maximum;
            Window.MinIntensity = image.Minimum;
        }

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
        {
            var voxel = image[x, y, 0];
            display.SetPixel(x, y, Window.GetColor(voxel));
        }
    }
}
