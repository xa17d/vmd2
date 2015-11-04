namespace Vmd2.Processing.Filters
{
    class Laplace2DFilter3x3 : Filter2D
    {
        private static double[,] array = new double[3, 3] { 
            { 1,  1, 1 }, 
            { 1, -8, 1 }, 
            { 1,  1, 1 }
        };

        public Laplace2DFilter3x3() : base(array)
        {
        }
    }
}
