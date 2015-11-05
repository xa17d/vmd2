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

        private bool axisX = false;
        public bool AxisX
        {
            get { return axisX; }
            set
            {
                if (value != axisX)
                {
                    axisX = value;
                    if (value) { AxisY = false; AxisZ = false; }
                    OnPropertyChanged();
                }
            }
        }

        private bool axisY = false;
        public bool AxisY
        {
            get { return axisY; }
            set
            {
                if (value != axisY)
                {
                    axisY = value;
                    if (value) { AxisX = false; AxisZ = false; }
                    OnPropertyChanged();
                }
            }
        }

        private bool axisZ = true;
        public bool AxisZ
        {
            get { return axisZ; }
            set
            {
                if (value != axisZ)
                {
                    axisZ = value;
                    if (value) { AxisX = false; AxisY = false; }
                    OnPropertyChanged();
                }
            }
        }

        private int sliceMax = 0;
        public int SliceMax
        {
            get { return sliceMax; }
            private set
            {
                if (value != sliceMax)
                {
                    sliceMax = value;

                    OnPropertyChanged();
                    if (sliceMax < sliceIndex)
                    {
                        SliceIndex = SliceMax;
                    }
                }
            }
        }

        protected override Image3D OnProcess(Image3D image, Progress progress)
        {
            Image3D result;

            if (axisX) // X-Axis
            {
                result = new Image3D(1, image.LengthZ, image.LengthY);
                result.Minimum = image.Minimum;
                result.Maximum = image.Maximum;

                SliceMax = image.LengthX - 1;

                for (int z = 0; z < image.LengthZ; z++)
                {
                    for (int y = 0; y < image.LengthY; y++)
                    {
                        result[y, z, 0] = image[sliceIndex, y, z];
                    }
                }
            }
            else if (axisY) // Y-Axis
            {
                result = new Image3D(1, image.LengthZ, image.LengthX);
                result.Minimum = image.Minimum;
                result.Maximum = image.Maximum;

                SliceMax = image.LengthY - 1;

                for (int z = 0; z < image.LengthZ; z++)
                {
                    for (int x = 0; x < image.LengthX; x++)
                    {
                        result[x, z, 0] = image[x, sliceIndex, z];
                    }
                }
            }
            else if (axisZ) // Z-Axis
            {
                result = new Image3D(1, image.LengthY, image.LengthX);
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
            }
            else { throw new LogException("no axis selected"); }

            return result;
        }

        public override string ToString()
        {
            return base.ToString() + " slice: " + sliceIndex;
        }
    }
}
