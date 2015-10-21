using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing.TransferFunctions
{
    class TransferFunctionRenderer
    {
        public TransferFunctionRenderer(Image3D image, DisplayImage display, TransferFunction1D tf)
        {
            this.image = image;
            this.display = display;
            this.tf = tf;
            this.Slice = 0;
        }

        public int Slice { get; set; }
        private Image3D image;
        private DisplayImage display;
        private TransferFunction1D tf;

        public void Render()
        {
            int s = Slice;

            for (int y = 0; y < image.LengthY; y++)
            {
                for (int x = 0; x < image.LengthX; x++)
                {
                    var voxel = image[x, y, s];
                    display.SetPixel(x, y, tf.GetColor(voxel));
                }
            }
        }
    }
}
