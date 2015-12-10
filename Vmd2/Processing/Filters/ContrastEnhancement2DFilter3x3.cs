namespace Vmd2.Processing.Filters
{
    class ContrastEnhancement2DFilter3x3 : Filter2D
    {
        private static double[,] array = new double[3, 3] {
                { -2, -2, -2 },
                { -2, 32, -2 },
                { -2, -2, -2 }
            };

        public ContrastEnhancement2DFilter3x3() : base(array)
        {
        }
    }
}
