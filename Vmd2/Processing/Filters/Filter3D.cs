using System;
using Vmd2.Logging;

namespace Vmd2.Processing.Filters
{
    abstract class Filter3D : ProcessingElement3D
    {
        private double[,,] filter;
        private int xCount;
        private int yCount;
        private int zCount;
        private double divider;

        public Filter3D(double[,,] filter) : base(8, false)
        {
            this.filter = filter;
            xCount = filter.GetLength(0);
            yCount = filter.GetLength(1);
            zCount = filter.GetLength(2);
            divider = GetDivider();
            Activated = true;

            if (xCount % 2 == 0 || yCount % 2 == 0 || zCount % 2 == 0)
            {
                throw new ArgumentException("illegal filter size");
            }
        }

        private bool activated;
        public bool Activated
        {
            get { return activated; }
            set { if (value != activated) { activated = value; OnPropertyChanged(); } }
        }

        public double FilterPixel(int x, int y, int z, Image3D image)
        {
            if (Activated)
            {
                double[,,] area = image.GetArea(x, y, z, xCount, yCount, zCount);

                double sum = 0;
                for (int i = 0; i < zCount; i++)
                {
                    for (int j = 0; j < yCount; j++)
                    {
                        for (int k = 0; k < xCount; k++)
                        {
                            sum += area[k, j, i] * filter[k, j, i];
                        }
                    }
                }

                return sum / divider;
            }

            return image[x, y, z];
        }

        private double GetDivider()
        {
            double sum = 0;

            for (int i = 0; i < zCount; i++)
            {
                for (int j = 0; j < yCount; j++)
                {
                    for (int k = 0; k < xCount; k++)
                    {
                        sum += filter[k, j, i];
                    }
                }
            }

            return sum != 0 ? sum : 1;
        }

        protected override Image3D OnProcess(Image3D image, Progress progress)
        {
            if (activated)
            {
                return base.OnProcess(image, progress);
            }
            else
            {
                progress.Done();
                return image;
            }
        }

        protected override void OnProcess3D(Image3D imageIn, Image3D imageOut, int x, int y, int z)
        {
            imageOut[x, y, z] = FilterPixel(x, y, z, imageIn);
        }
    }
}
