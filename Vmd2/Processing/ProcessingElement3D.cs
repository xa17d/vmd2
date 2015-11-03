using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing
{
    abstract class ProcessingElement3D : ProcessingElement2D
    {
        protected override void OnProcess2D(Image3D imageIn, Image3D imageOut, int x, int y)
        {
            for (int z = 0; z < imageIn.LengthZ; z++)
            {
                OnProcess3D(imageIn, imageOut, x, y, z);
            }
        }

        protected abstract void OnProcess3D(Image3D imageIn, Image3D imageOut, int x, int y, int z);
    }
}
