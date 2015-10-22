using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing
{
    class Image3D
    {
        private short[,,] data;

        public int LengthX { get; private set; }
        public int LengthY { get; private set; }
        public int LengthZ { get; private set; }

        public Image3D(int lengthZ, int lengthY, int lengthX)
        {
            data = new short[lengthX, lengthY, lengthZ];
            this.LengthX = lengthX;
            this.LengthY = lengthY;
            this.LengthZ = lengthZ;
        }

        public double this[int x, int y, int z]
        {
            get { return data[x, y, z]; }
            set { data[x, y, z] = (short)value; }
        }
    }
}
