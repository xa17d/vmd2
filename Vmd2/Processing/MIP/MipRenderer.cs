using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Vmd2.Logging;
using Vmd2.Processing.Helper;
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Processing.DVR
{
    class MipRenderer : RendererPixel
    {
        public MipRenderer(Image3D image, DisplayImage display, Windowing window) : base(display)
        {
            this.image = image;
            this.display = display;
            this.window = window;
        }

        private Image3D image;
        private DisplayImage display;
        private Windowing window;

        protected override void OnRenderPixel(int x, int y)
        {
            double mipValue = double.MinValue;
            for (int z = image.LengthZ - 1; z >= 0; z--)
            {
                mipValue = Math.Max(mipValue, image[x, y, z]);
            }

            display.SetPixel(x, y, window.GetColor(mipValue));
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
