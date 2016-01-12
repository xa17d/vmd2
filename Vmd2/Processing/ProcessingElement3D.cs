using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing
{
    /// <summary>
    /// ProcessingElement with OnProcess3D-Method is called for every voxel
    /// </summary>
    abstract class ProcessingElement3D : ProcessingElement2D
    {
        public ProcessingElement3D() : base() { }
        public ProcessingElement3D(int threadCount) : base(threadCount) { }

        protected override void OnProcess2D(Image3D imageIn, Image3D imageOut, int x, int y)
        {
            for (int z = 0; z < imageIn.LengthZ; z++)
            {
                OnProcess3D(imageIn, imageOut, x, y, z);
            }
        }

        /// <summary>
        /// Is called for every voxel of the input image
        /// </summary>
        /// <param name="imageIn">input image</param>
        /// <param name="imageOut">output image</param>
        /// <param name="x">x</param>
        /// <param name="y">y</param>
        /// <param name="z">z</param>
        protected abstract void OnProcess3D(Image3D imageIn, Image3D imageOut, int x, int y, int z);
    }
}
