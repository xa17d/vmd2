using System;
using Vmd2.Logging;

namespace Vmd2.Processing.Filters
{
    abstract class Filter : ProcessingElement3D
    {
        private double[,] filter;
        private int rowCount;
        private int columnCount;
        private double divider;

        public Filter(double[,] filter)
        {
            this.filter = filter;
            rowCount = filter.GetLength(0);
            columnCount = filter.GetLength(1);
            divider = GetDivider();
            Activated = true;

            if (rowCount % 2 == 0 || columnCount % 2 == 0)
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
                double[,] area = image.GetArea(x, y, z, columnCount, rowCount);

                double sum = 0;
                for (int i = 0; i < columnCount; i++)
                {
                    for (int j = 0; j < rowCount; j++)
                    {
                        sum += area[j, i] * filter[j, i];
                    }
                }

                return sum / divider;
            }

            return image[x, y, z];
        }

        private double GetDivider()
        {
            double sum = 0;

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    sum += filter[j, i];
                }
            }

            //avoid dividing by zero
            if (sum == 0)
            {
                sum = 1;
            }

            return sum;
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
