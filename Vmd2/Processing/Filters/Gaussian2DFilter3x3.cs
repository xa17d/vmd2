namespace Vmd2.Processing.Filters
{
    class Gaussian2DFilter3x3 : Filter2D
    {
        private static double[,] array = new double[3, 3] {
            { 1, 2, 1 },
            { 2, 4, 2 },
            { 1, 2, 1 }
        };

        public Gaussian2DFilter3x3() : base(array)
        {
        }
    }
}
