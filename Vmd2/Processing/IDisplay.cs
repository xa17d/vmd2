﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Vmd2.Processing
{
    interface IDisplay
    {
        DisplayImage GetDisplay(int width, int height);
        Point Marker { get; }
    }
}
