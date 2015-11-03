using System;
using Vmd2.Processing.Mapping;

namespace Vmd2.Processing.MIP
{
    class MipRenderer : Renderer
    {
        public MipRenderer(Windowing window)
        {

            this.window = window;
        }

        private Windowing window;

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
        {
            double mipValue = double.MinValue;
            for (int z = image.LengthZ - 1; z >= 0; z--)
            {
                mipValue = Math.Max(mipValue, image[x, y, z]);
            }

            display.SetPixel(x, y, window.GetColor(mipValue));
        }
    }
}
