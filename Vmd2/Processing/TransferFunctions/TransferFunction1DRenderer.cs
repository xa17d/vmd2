using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing.TransferFunctions
{
    class TransferFunction1DRenderer : RendererPixel
    {
        public TransferFunction1DRenderer(Image3D image, DisplayImage display, TransferFunction1D tf) : base(display)
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

        protected override void OnRenderPixel(int x, int y)
        {
            var voxel = image[x, y, Slice];
            display.SetPixel(x, y, tf.GetColor(voxel));
        }

        /*public void Render()
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
        }*/
    }
}
