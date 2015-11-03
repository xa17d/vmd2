namespace Vmd2.Processing.Filter
{
    class GradientFilter3x3 : FilterRenderer
    {
        // 1  1  1
        // 1 -8  1
        // 1  1  1
        private static double[,] array = new double[3, 3] { { 1, 1, 1 }, { 1, -8, 1 }, { 1, 1, 1 } };
        public GradientFilter3x3() : base(array)
        {
        }
    }
}
