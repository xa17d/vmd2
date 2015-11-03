﻿namespace Vmd2.Processing.Filter
{
    class GaussianFilter3x3 : FilterRenderer
    {
        // 1 2 1
        // 2 4 2
        // 1 2 1
        private static double[,] array = new double[3, 3] { { 1, 2, 1 }, { 2, 4, 2 }, { 1, 2, 1 } };
        public GaussianFilter3x3() : base(array)
        {
        }
    }
}
