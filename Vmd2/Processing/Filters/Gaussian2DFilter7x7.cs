namespace Vmd2.Processing.Filters
{
    class Gaussian2DFilter7x7 : Filter2D
    {
        private static double[,] array = new double[7, 7] { 
            { 1, 2,  3,  4,  3, 2, 1 }, 
            { 2, 4,  6,  8,  6, 4, 2 }, 
            { 3, 6,  9, 12,  9, 6, 3 }, 
            { 4, 8, 12, 16, 12, 8, 4 }, 
            { 3, 6,  9, 12,  9, 6, 3 }, 
            { 2, 4,  6,  8,  6, 4, 2 }, 
            { 1, 2,  3,  4,  3, 2, 1 }
        };

        public Gaussian2DFilter7x7() : base(array)
        {
        }
    }
}