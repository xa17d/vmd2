using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media;

namespace Vmd2.Processing.TransferFunctions
{
    class TransferFunction1DRenderer : Renderer
    {
        private TransferFunction1D tf = TransferFunction1D.Default;
        public TransferFunction1D TF
        {
            get { return tf; }
            set { if (value != tf) { tf = value; OnPropertyChanged(); } }
        }

        protected override void OnValidate(Image3D image)
        {
            if (image.LengthZ != 1) { throw new LogException("can only render Images with exactly one slice"); }
        }

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
        {
            var voxel = image[x, y, 0];
            display.SetPixel(x, y, tf.GetColor(voxel));
        }
    }
}
