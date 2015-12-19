namespace Vmd2.Processing.Filters
{
    class ContrastEnhancement2DFilter3x3 : Filter2D
    {
        private static double[,] array = new double[3, 3] {
                { -0.5, -0.5, -0.5 },
                { -0.5,  5.0, -0.5 },
                { -0.5, -0.5, -0.5 }
            };

        public ContrastEnhancement2DFilter3x3() : base(array)
        {
        }
    }
}
