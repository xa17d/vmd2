namespace Vmd2.Processing.Filter
{
    class GaussianFilter7x7 : FilterRenderer
    {
        // 1  2  3   4  3  2  1
        // 2  4  6   8  6  4  2
        // 3  6  9  12  9  6  3
        // 4  8 12  16 12  8  4
        // 3  6   9 12  9  6  3
        // 2  4   6  8  6  4  2
        // 1  2   3  4  3  2  1
        private static double[,] array = new double[7, 7] { { 1, 2, 3, 4, 3, 2, 1 }, { 2, 4, 6, 8, 6, 4, 2 }, { 3, 6, 9, 12, 9, 6, 3 }, { 4, 8, 12, 16, 12, 8, 4 }, { 3, 6, 9, 12, 9, 6, 3 }, { 2, 4, 6, 8, 6, 4, 2 }, { 1, 2, 3, 4, 3, 2, 1 } };
        public GaussianFilter7x7() : base(array)
        {
        }
    }
}
