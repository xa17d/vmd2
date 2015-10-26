using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Vmd2.Processing.Helper;
using Vmd2.Processing.TransferFunctions;

namespace Vmd2.Processing.DVR
{
    class DvrRenderer : RendererPixel
    {
        public DvrRenderer(Image3D image, DisplayImage display, TransferFunction1D tf) : base(display)
        {
            this.image = image;
            this.display = display;
            this.tf = tf;
        }

        private Image3D image;
        private DisplayImage display;
        private TransferFunction1D tf;

        protected override void OnRenderPixel(int x, int y)
        {
            var voxelColor = Colors.Black;
            for (int z = image.LengthZ - 1; z >= 0; z--)
            {
                var color = tf.GetColor(image[x, y, z]);
                voxelColor = ColorHelper.Blend(color, voxelColor, color.A / 255.0);
            }

            display.SetPixel(x, y, voxelColor);
        }
    }
}
