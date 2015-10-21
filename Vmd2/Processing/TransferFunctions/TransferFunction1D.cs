using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing.TransferFunctions
{
    class TransferFunction1D
    {
        public Color GetColor(double value)
        {
            // test implementation
            int v = (int)(value / 1100.0 * 255.0);
            return Color.FromArgb(255, v, v, v);
        }
    }
}
