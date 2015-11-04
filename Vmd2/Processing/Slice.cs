using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Vmd2.Logging;

namespace Vmd2.Processing
{
    class Slice : ProcessingElement
    {
        private int sliceIndex = 0;
        public int SliceIndex
        {
            get { return sliceIndex; }
            set { if (value != sliceIndex) { sliceIndex = value; OnPropertyChanged(); } }
        }

        private int sliceMax = 0;
        public int SliceMax
        {
            get { return sliceMax; }
            private set { if (value != sliceMax) { sliceMax = value; OnPropertyChanged(); } }
        }

        protected override Image3D OnProcess(Image3D image, Progress progress)
        {
            Image3D result = new Image3D(1, image.LengthY, image.LengthX);
            result.Minimum = image.Minimum;
            result.Maximum = image.Maximum;

            SliceMax = image.LengthZ - 1;

            for (int y = 0; y < image.LengthY; y++)
            {
                for (int x = 0; x < image.LengthX; x++)
                {
                    result[x, y, 0] = image[x, y, sliceIndex];
                }
            }

            return result;
        }

        public override string ToString()
        {
            return base.ToString() + " slice: " + sliceIndex;
        }
    }
}
