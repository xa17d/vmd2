using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing.Filter
{
    class GaussianFilter
    {
        private double[,] filter;
        private int rowCount;
        private int columnCount;
        private double divider;
        public GaussianFilter(double[,] filter)
        {
            this.filter = filter;
            this.rowCount = filter.GetLength(0);
            this.columnCount = filter.GetLength(1);
            this.divider = GetDivider();
            this.Activated = true;

            if (rowCount % 2 == 0 || columnCount % 2 == 0)
            {
                throw new ArgumentException("illegal filter size");
            }
        }

        public bool Activated { set; get; }

        public double FilterPixel(int x, int y, int z, Image3D image)
        {
            if(Activated)
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

            return image[x,y,z];
        }

        private double GetDivider()
        {
            double sum = 0;

            for (int i = 0; i < columnCount; i++)
            {
                for (int j = 0; j < rowCount; j++)
                {
                    sum += filter[j,i];
                }
            }

            return sum;
        }
    }
}
