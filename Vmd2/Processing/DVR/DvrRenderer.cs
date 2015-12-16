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
        public DvrRenderer()
        {
            TF = TransferFunction1DBuilder.CreateTestBuilder();
        }

        private TransferFunction1DBuilder tfBuilder;
        public TransferFunction1DBuilder TF
        {
            get { return tfBuilder; }
            set
            {
                if (value != tfBuilder)
                {
                    tfBuilder = value;
                    OnPropertyChanged();
                }
            }
        }

        private TransferFunction1D tf;

        protected override void OnValidate(Image3D image)
        {
            base.OnValidate(image);
            tf = tfBuilder.CreateTransferFunction();
        }

        protected override void OnRenderPixel(Image3D image, DisplayImage display, int x, int y)
        {
            var voxelColor = Colors.Black;
            for (int z = image.LengthZ - 1; z >= 0; z--)
            {
                var color = tf.GetColor(image[x, y, z]);
                voxelColor = ColorHelper.BlendAlpha(color, voxelColor);
            }

            display.SetPixel(x, y, voxelColor);
        }
    }
}
