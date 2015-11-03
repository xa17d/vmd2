using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Vmd2.Processing
{
    interface IDisplay
    {
        DisplayImage GetDisplay(int width, int height);
    }
}
