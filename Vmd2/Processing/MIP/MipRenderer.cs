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
        public MipRenderer(Image3D image, DisplayImage display, double min, double max) : base(display)
        {
            this.image = image;
            this.display = display;
            this.min = min;
            this.max = max;
        }

        private Image3D image;
        private DisplayImage display;
        private double min;
        private double max;

        protected override void OnRenderPixel(int x, int y)
        {
            double resultColor = min;
            for (int z = image.LengthZ - 1; z >= 0; z--)
            {
                double color = image[x, y, z];
                if(color > resultColor)
                {
                    resultColor = color;
                }
            }
            byte rgb = Convert.ToByte((resultColor - min) / (max - min) * 0xFF / 3);
            display.SetPixel(x, y, Color.FromArgb(0xFF, rgb, rgb, rgb));
        }

        public override string ToString()
        {
            return GetType().Name;
        }
    }
}
