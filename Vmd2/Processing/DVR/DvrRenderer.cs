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
    class DvrRenderer : Renderer
    {
        private TransferFunction1D tf = TransferFunction1D.Default;
        public TransferFunction1D TF
        {
            get { return tf; }
            set { if (value != tf) { tf = value; OnPropertyChanged(); } }
        }

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
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
