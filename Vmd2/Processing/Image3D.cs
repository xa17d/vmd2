using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing
{
    public class Image3D
    {
        private short[][,] data;

        public int LengthX { get; private set; }
        public int LengthY { get; private set; }
        public int LengthZ { get; private set; }

        public double Maximum { get; set; }
        public double Minimum { get; set; }

        public Image3D(int lengthZ, int lengthY, int lengthX)
        {
            if (lengthZ > 1000) { lengthZ = 1000; } // TODO: remove

            data = new short[lengthZ][,];

            for (int i = 0; i < lengthZ; i++)
            {
                data[i] = new short[lengthX, lengthY];
            }

            this.LengthX = lengthX;
            this.LengthY = lengthY;
            this.LengthZ = lengthZ;
        }

        public Image3D EmptyCopy()
        {
            var result = new Image3D(LengthZ, LengthY, LengthX);
            result.Minimum = Minimum;
            result.Maximum = Maximum;
            return result;
        }

        public double this[int x, int y, int z]
        {
            get { return data[z][x, y]; }
            set { data[z][x, y] = (short)value; }
        }

        public double[,] GetArea(int x, int y, int z, int xLength, int yLength)
        {
            if (xLength % 2 == 0 || yLength % 2 == 0)
            {
                throw new ArgumentException("illegal filter size");
            }

            int xl = Convert.ToInt32(Math.Floor((decimal)xLength / 2));
            int yl = Convert.ToInt32(Math.Floor((decimal)yLength / 2));

            double[,] area = new double[xLength, yLength];
            for (int i = x - xl; i <= x + xl; i++)
            {
                for (int j = y - yl; j <= y + yl; j++)
                {
                    if (i < 0 || i >= LengthX)
                    {
                        area[i - x + xl, j - y + yl] = 0;
                    }
                    else if (j < 0 || j >= LengthY)
                    {
                        area[i - x + xl, j - y + yl] = 0;
                    }
                    else
                    {
                        area[i - x + xl, j - y + yl] = this[i, j, z];
                    }
                }
            }

            return area;
        }

        public double[,,] GetArea(int x, int y, int z, int xLength, int yLength, int zLength)
        {
            int xl = Convert.ToInt32(Math.Floor((decimal)xLength / 2));
            int yl = Convert.ToInt32(Math.Floor((decimal)yLength / 2));
            int zl = Convert.ToInt32(Math.Floor((decimal)zLength / 2));

            double[,,] area = new double[xLength, yLength, zLength];
            for (int k = z - zl; k <= z + zl; k++)
            {
                for (int i = x - xl; i <= x + xl; i++)
                {
                    for (int j = y - yl; j <= y + yl; j++)
                    {
                        if (i < 0 || i >= LengthX)
                        {
                            area[i - x + xl, j - y + yl, k - z + zl] = 0;
                        }
                        else if (j < 0 || j >= LengthY)
                        {
                            area[i - x + xl, j - y + yl, k - z + zl] = 0;
                        }
                        else if (k < 0 || k >= zLength)
                        {
                            area[i - x + xl, j - y + yl, k - z + zl] = 0;
                        }
                        else
                        {
                            area[i - x + xl, j - y + yl, k - z + zl] = this[i, j, k];
                        }
                    }
                }
            }

            return area;
        }

        public static string InfoString(Image3D image)
        {
            if (image == null) { return "null"; }
            else
            {
                return image.LengthX + "x" + image.LengthY + "x" + image.LengthZ;
            }
        }
    }
}
