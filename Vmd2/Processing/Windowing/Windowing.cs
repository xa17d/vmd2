﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Media;
using Vmd2.Processing.Helper;

namespace Vmd2.Processing.TransferFunctions
{
    public class Windowing
    {
        public Windowing()
        {
            // default values
            WindowCenter = 500;
            WindowWidth = 300;
        }

        public double WindowCenter { get; set; }
        public double WindowWidth { get; set; }

        public Color GetColor(double value)
        {
            // see http://www.codeproject.com/Articles/466955/Medical-image-visualization-using-WPF
            byte rgb;
            if (value < WindowCenter - WindowWidth / 2)
            {
                //black
                rgb = 0x00;
            }
            else if (value > WindowCenter + WindowWidth / 2)
            {
                //white
                rgb = 0xFF;
            }
            else
            {
                //grey
                rgb = Convert.ToByte((value - (WindowCenter - WindowWidth / 2)) / WindowWidth * 0xFF);
            }

            return Color.FromArgb(0xFF, rgb, rgb, rgb);
        }
    }
}