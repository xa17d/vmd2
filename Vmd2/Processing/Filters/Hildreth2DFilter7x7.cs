namespace Vmd2.Processing.Filters
{
    class Hildreth2DFilter7x7 : Filter2D
    {
        private static double[,] array = new double[7, 7] { 
            { 1, 3,   4,   4,   4, 3, 1 }, 
            { 3, 4,   3,   0,   3, 4, 3 }, 
            { 4, 3,  -9, -17,  -9, 3, 4 }, 
            { 4, 0, -17, -30, -17, 0, 4 },
            { 4, 3,  -9, -17,  -9, 3, 4 },
            { 3, 4,   3,   0,   3, 4, 3 },
            { 1, 3,   4,   4,   4, 3, 1 }
        };

        public Hildreth2DFilter7x7() : base(array)
        {
        }
    }
}