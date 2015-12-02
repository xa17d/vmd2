using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing
{
    class TransformYView : ProcessingElement3D
    {
        protected override Image3D GetOutputImage(Image3D imageIn)
        {
            var imageOut = new Image3D(imageIn.LengthY, imageIn.LengthZ, imageIn.LengthX);
            imageOut.Minimum = imageIn.Minimum;
            imageOut.Maximum = imageIn.Maximum;
            return imageOut;
        }

        protected override void OnProcess3D(Image3D imageIn, Image3D imageOut, int x, int y, int z)
        {
            imageOut[x, z, y] = imageIn[x, y, z];
        }
    }
}
