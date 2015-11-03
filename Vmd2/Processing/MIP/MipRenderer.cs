using System;
using Vmd2.Processing.Mapping;

namespace Vmd2.Processing.MIP
{
    class MipRenderer : Renderer
    {
        public MipRenderer()
        {
            this.Window = new Windowing();
            this.Window.PropertyChanged += Window_PropertyChanged;
        }

        private void Window_PropertyChanged(object sender, System.ComponentModel.PropertyChangedEventArgs e)
        {
            OnPropertyChanged("Window");
        }

        public Windowing Window { private set; get; }

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
        {
            double mipValue = double.MinValue;
            for (int z = image.LengthZ - 1; z >= 0; z--)
            {
                mipValue = Math.Max(mipValue, image[x, y, z]);
            }

            display.SetPixel(x, y, Window.GetColor(mipValue));
        }
    }
}
